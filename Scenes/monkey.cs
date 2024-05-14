using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class monkey : MonoBehaviour
{
    public Transform target; // موقع الموزة
    public float speed = 5f;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }
    public class PriorityQueue<T>
    {
        private List<Tuple<T, float>> elements = new List<Tuple<T, float>>();

        public int Count
        {
            get { return elements.Count; }
        }

        public void Enqueue(T item, float priority)
        {
            elements.Add(Tuple.Create(item, priority));
        }

        public T Dequeue()
        {
            int bestIndex = 0;

            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].Item2 < elements[bestIndex].Item2)
                {
                    bestIndex = i;
                }
            }

            T bestItem = elements[bestIndex].Item1;
            elements.RemoveAt(bestIndex);
            return bestItem;
        }
    }



    private List<Node> openList = new List<Node>(); // قائمة العقد المفتوحة
    private List<Node> closedList = new List<Node>(); // قائمة العقد المغلقة

    private void Start()
    {
        // بدأ البحث
        StartCoroutine(SearchAlgorithms(transform.position));
    }

    private IEnumerator SearchAlgorithms(Vector3 startPosition)
    {
        // تنفيذ جميع خوارزميات البحث بتوالي
        yield return StartCoroutine(DFS(startPosition));
        yield return StartCoroutine(DLS(startPosition, 3)); // العمق المحدد هنا 3 كونها مثال
        yield return StartCoroutine(BFS(startPosition));
        yield return StartCoroutine(AStar(startPosition));
        yield return StartCoroutine(GreedyBestFirstSearch(startPosition));
    }

    private IEnumerator DFS(Vector3 startPosition)
    {
        // خوارزمية البحث في العمق
        Stack<Node> stack = new Stack<Node>();
        Node startNode = new Node(startPosition);
        stack.Push(startNode);

        while (stack.Count > 0)
        {
            Node currentNode = stack.Pop();
            if (currentNode.position == target.position)
            {
                Debug.Log("DFS: Found target!");
                yield break;
            }

            // احصل على العقد المجاورة وأضفها إلى القائمة المفتوحة
            foreach (Node adjacentNode in GetAdjacentNodes(currentNode))
            {
                if (!closedList.Contains(adjacentNode))
                {
                    stack.Push(adjacentNode);
                    closedList.Add(adjacentNode);
                }
            }
        }

        Debug.Log("DFS: Target not found!");
        yield break;
    }

    private IEnumerator DLS(Vector3 startPosition, int depthLimit)
    {
        // خوارزمية البحث بالعمق المحدد
        Stack<Node> stack = new Stack<Node>();
        Node startNode = new Node(startPosition);
        stack.Push(startNode);

        int depth = 0;

        while (stack.Count > 0 && depth <= depthLimit)
        {
            Node currentNode = stack.Pop();
            if (currentNode.position == target.position)
            {
                Debug.Log("DLS: Found target!");
                yield break;
            }

            // احصل على العقد المجاورة وأضفها إلى القائمة المفتوحة
            foreach (Node adjacentNode in GetAdjacentNodes(currentNode))
            {
                if (!closedList.Contains(adjacentNode))
                {
                    stack.Push(adjacentNode);
                    closedList.Add(adjacentNode);
                }
            }

            depth++;
        }

        Debug.Log("DLS: Target not found within depth limit!");
        yield break;
    }

    private IEnumerator BFS(Vector3 startPosition)
    {
        // خوارزمية البحث في العرض
        Queue<Node> queue = new Queue<Node>();
        Node startNode = new Node(startPosition);
        queue.Enqueue(startNode);

        while (queue.Count > 0)
        {
            Node currentNode = queue.Dequeue();
            if (currentNode.position == target.position)
            {
                Debug.Log("BFS: Found target!");
                yield break;
            }

            // احصل على العقد المجاورة وأضفها إلى القائمة المفتوحة
            foreach (Node adjacentNode in GetAdjacentNodes(currentNode))
            {
                if (!closedList.Contains(adjacentNode))
                {
                    queue.Enqueue(adjacentNode);
                    closedList.Add(adjacentNode);
                }
            }
        }

        Debug.Log("BFS: Target not found!");
        yield break;
    }

    private IEnumerator AStar(Vector3 startPosition)


    {
        // خوارزمية البحث A*
        PriorityQueue<Node> priorityQueue = new PriorityQueue<Node>();
        Node startNode = new Node(startPosition);
        startNode.g = 0;
        startNode.h = Vector3.Distance(startPosition, target.position);
        startNode.f = startNode.g + startNode.h;
        priorityQueue.Enqueue(startNode, startNode.f);

        while (priorityQueue.Count > 0)
        {
            Node currentNode = priorityQueue.Dequeue();
            if (currentNode.position == target.position)
            {
                Debug.Log("A*: Found target!");
                yield break;
            }

            // احصل على العقد المجاورة وقم بتحديث التكلفة وإضافتها إلى القائمة المفتوحة
            foreach (Node adjacentNode in GetAdjacentNodes(currentNode))
            {
                float newG = currentNode.g + Vector3.Distance(currentNode.position, adjacentNode.position);
                float newH = Vector3.Distance(adjacentNode.position, target.position);
                float newF = newG + newH;

                if (!closedList.Contains(adjacentNode) || newF < adjacentNode.f)
                {
                    adjacentNode.g = newG;
                    adjacentNode.h = newH;
                    adjacentNode.f = newF;

                    if (!closedList.Contains(adjacentNode))
                    {
                        priorityQueue.Enqueue(adjacentNode, adjacentNode.f);
                        closedList.Add(adjacentNode);
                    }
                }
            }
        }

        Debug.Log("A*: Target not found!");
        yield break;
    }

    private IEnumerator GreedyBestFirstSearch(Vector3 startPosition)
    {
        // خوارزمية البحث الأنانية بأفضل الطريقة
        PriorityQueue<Node> priorityQueue = new PriorityQueue<Node>();
        Node startNode = new Node(startPosition);
        startNode.h = Vector3.Distance(startPosition, target.position);
        priorityQueue.Enqueue(startNode, startNode.h);

        while (priorityQueue.Count > 0)
        {
            Node currentNode = priorityQueue.Dequeue();
            if (currentNode.position == target.position)
            {
                Debug.Log("Greedy: Found target!");
                yield break;
            }

            // احصل على العقد المجاورة وأضفها إلى القائمة المفتوحة
            foreach (Node adjacentNode in GetAdjacentNodes(currentNode))
            {
                if (!closedList.Contains(adjacentNode))
                {
                    priorityQueue.Enqueue(adjacentNode, adjacentNode.h);
                    closedList.Add(adjacentNode);
                }
            }
        }

        Debug.Log("Greedy: Target not found!");
        yield break;
    }

    private List<Node> GetAdjacentNodes(Node node)
    {
        // احصل على العقد المجاورة للعقد الحالي
        // يمكن تعديل هذه الدالة وفقًا لهيكل اللعبة والمتطلبات
        List<Node> adjacentNodes = new List<Node>();

        // مثال على كيفية الحصول على العقد المجاورة بناءً على شبكة نقاط
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
        foreach (Vector3 direction in directions)
        {
            Vector3 newPosition = node.position + direction;
            Node adjacentNode = new Node(newPosition);
            adjacentNodes.Add(adjacentNode);
        }

        return adjacentNodes;
    }
}

public class Node
{
    public Vector3 position;
    public float g; // التكلفة الفعلية للوصول إلى هذا العقد من العقد الأولي
    public float h; // التقدير الحالي للتكلفة للوصول من هذا العقد إلى الموقع المستهدف
    public float f; // المجموع (g + h)

    public Node(Vector3 pos)
    {
        position = pos;
    }
}

