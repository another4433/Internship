namespace Data_Structure
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            LWLinkedList<String> list = new LWLinkedList<String>();
            MWArrayList<Int32> list2 = new MWArrayList<Int32>();
            list.add("Hello");
            list.add("World");
            list2.add(0);
            list2.add(1);
            list2.add(2);
            list.printList();
            list2.printList();
        }
    }

    public class LWLinkedList<TE>
    {
        public class TheNode<TE>
        {
            public TE theData;
            public TheNode<TE>? next, previous;

            public TheNode(TE data)
            {
                theData = data;
                next = null;
                previous = null;
            }

            public TE getData()
            {
                return theData;
            }
        }

        public TheNode<TE>? head, tail;
        int size;

        public LWLinkedList()
        {
            head = null;
            tail = null;
            size = 0;
        }

        public void add(TE data)
        {
            TheNode<TE> newNode = new TheNode<TE>(data);
            if (head == null)
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                tail!.next = newNode;
                newNode.previous = tail;
                tail = newNode;
            }
            size++;
        }
        public void remove(TE data)
        {
            TheNode<TE>? current = head;
            while (current != null)
            {
                if (current.theData!.Equals(data))
                {
                    if (current.previous != null)
                    {
                        current.previous.next = current.next;
                    }
                    else
                    {
                        head = current.next; // Update head if removing the first node
                    }
                    if (current.next != null)
                    {
                        current.next.previous = current.previous;
                    }
                    else
                    {
                        tail = current.previous; // Update tail if removing the last node
                    }
                    size--;
                    return;
                }
                current = current.next;
            }
        }
        public int getSize()
        {
            return size;
        }
        public bool isEmpty()
        {
            return size == 0;
        }
        public void clear()
        {
            head = null;
            tail = null;
            size = 0;
        }
        public bool contains(TE data)
        {
            TheNode<TE>? current = head;
            while (current != null)
            {
                if (current.theData!.Equals(data))
                {
                    return true;
                }
                current = current.next;
            }
            return false;
        }
        public void printList()
        {
            TheNode<TE>? current = head;
            while (current != null)
            {
                Console.Write(current.theData + " ");
                current = current.next;
            }
            Console.WriteLine();
        }
        public void replace(TE oldData, TE newData)
        {
            TheNode<TE>? current = head;
            while (current != null)
            {
                if (current.theData!.Equals(oldData))
                {
                    current.theData = newData;
                    return;
                }
                current = current.next;
            }
        }
        public TE getFirst()
        {
            return head!.getData();
        }
        public TE getLast()
        {
            return tail!.getData();
        }
        public TE get(int index)
        {
            if (index < 0 || index >= size)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index out of bounds");
            }
            TheNode<TE>? current = head;
            for (int i = 0; i < index; i++)
            {
                current = current!.next;
            }
            return current!.getData();
        }
        public int search(TE item)
        {
            TheNode<TE>? current = head;
            int index = 0;
            while (current != null)
            {
                if (current.theData!.Equals(item))
                {
                    return index;
                }
                current = current.next;
                index++;
            }
            return -1; // Item not found
        }
        public void insert(TE item, int loc)
        {
            if (loc < 0 || loc > size)
            {
                throw new ArgumentOutOfRangeException(nameof(loc), "Index out of bounds");
            }
            TheNode<TE> newNode = new TheNode<TE>(item);
            if (loc == 0)
            {
                newNode.next = head;
                if (head != null)
                {
                    head.previous = newNode;
                }
                head = newNode;
                tail ??= newNode;
            }
            else if (loc == size)
            {
                tail!.next = newNode;
                newNode.previous = tail;
                tail = newNode;
            }
            else
            {
                TheNode<TE>? current = head;
                for (int i = 0; i < loc - 1; i++)
                {
                    current = current!.next;
                }
                newNode.next = current!.next;
                newNode.previous = current;
                if (current.next != null)
                {
                    current.next.previous = newNode;
                }
                current.next = newNode;
            }
            size++;
        }
    }
    public class MWArrayList<TE>
    {
        private int size, capacity;
        private TE[] elements;

        public MWArrayList(int initialCapacity = 10)
        {
            capacity = initialCapacity;
            size = 0;
            elements = new TE[capacity];
        }
        public void resize()
        {
            capacity *= 2;
            TE[] newElements = new TE[capacity];
            for (int i = 0; i < size; i++)
            {
                newElements[i] = elements[i];
            }
            elements = newElements;
        }
        public int getSize()
        {
            return size;
        }
        public int getCapacity()
        {
            return capacity;
        }
        public void add(TE item)
        {
            if (getSize() == getCapacity())
            {
                resize();
            }
            elements[size] = item;
            size++;
        }
        public void remove(TE item)
        {
            int index = search(item);
            if (index != -1)
            {
                for (int i = index; i < size - 1; i++)
                {
                    elements[i] = elements[i + 1];
                }
                elements[size - 1] = default(TE)!; // Clear the last element
                size--;
            }
        }
        public int search(TE item)
        {
            for (int i = 0; i < size; i++)
            {
                if (elements[i]!.Equals(item))
                {
                    return i;
                }
            }
            return -1; // Item not found
        }
        public TE get(int index)
        {
            if (index < 0 || index >= size)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index out of bounds");
            }
            return elements[index]!;
        }
        public void replace(TE oldItem, TE newItem)
        {
            int index = search(oldItem);
            if (index != -1)
            {
                elements[index] = newItem;
            }
        }
        public bool contains(TE item)
        {
            return search(item) != -1;
        }
        public void clear()
        {
            size = 0;
            elements = new TE[capacity]; // Reset the array to its initial capacity
        }
        public bool isEmpty()
        {
            return size == 0;
        }
        public void printList()
        {
            for (int i = 0; i < size; i++)
            {
                Console.Write(elements[i] + " ");
            }
            Console.WriteLine();
        }
    }
    public class Stack<TE>
    {
        int size;
        MWArrayList<TE> list;
        public int getSize()
        {
            return size;
        }
        public Stack()
        {
            size = -1;
            list = new MWArrayList<TE>();
        }
        public void push(TE item)
        {
            if (isFull())
            {
                list.resize(); // Resize the list if it is full
            }
            list.add(item);
            size++;
        }
        public TE pop()
        {
            if (isEmpty())
            {
                throw new InvalidOperationException("Stack is empty");
            }
            TE item = list.get(size);
            list.remove(item);
            size--;
            return item;
        }
        public bool isEmpty()
        {
            return size == -1;
        }
        public TE peek()
        {
            if (isEmpty())
            {
                throw new InvalidOperationException("Stack is empty");
            }
            return list.get(size);
        }
        public void clear()
        {
            list.clear();
            size = -1;
        }
        public void printStack()
        {
            list.printList();
            Console.WriteLine("Top of the stack: " + size);
        }
        public bool contains(TE item)
        {
            return list.contains(item);
        }
        public bool isFull()
        {
            return size == list.getCapacity() - 1; // Assuming the stack is full when capacity equals the last index of the list
        }
    }
    public class Queue<TE>
    {
        private int size;
        private LWLinkedList<TE>? list;
        private LWLinkedList<TE>.TheNode<TE>? front, rear;
        public Queue()
        {
            size = 0;
            list = new LWLinkedList<TE>();
            front = null;
            rear = null;
        }
        public void offer(TE item)
        {
            list!.add(item);
            if (front == null)
            {
                front = list.head;
                rear = list.head;
            }
            else if (front == rear)
            {
                rear = list.tail;
                rear!.previous = front;
                front!.next = rear;
            }
            else if (front!.next == rear && rear!.previous == front)
            {
                rear = list.tail;
                rear!.previous = front.next;
                front.next = rear.previous;
            }
            else
            {
                LWLinkedList<TE>.TheNode<TE>? newNode = front;
                while (newNode!.next != rear)
                {
                    if (newNode == rear!.previous)
                    {
                        rear!.previous = newNode;
                        newNode.next = rear;
                        break;
                    }
                    else
                    {
                        newNode = newNode.next;
                    }
                }
                rear = list.tail;
                newNode = newNode.next;
                rear!.previous = newNode;
                newNode!.next = rear;
                while (newNode!.previous != null)
                {
                    newNode = newNode.previous;
                }
                front = newNode;
            }
            size++;
        }
        public TE take()
        {
            TE backup = front!.getData();
            list!.remove(front!.getData()!);
            front = front.next;
            size--;
            return backup;
        }
        public TE element()
        {
            return front!.getData()!;
        }
        public int getSize()
        {
            return size;
        }
        public bool isEmpty()
        {
            return size == 0;
        }
        public void clear()
        {
            list!.clear();
            front = null;
            rear = null;
            size = 0;
        }
        public void printQueue()
        {
            list!.printList();
            Console.WriteLine("Front of the queue: " + (front != null ? front.getData() : "null"));
            Console.WriteLine("Rear of the queue: " + (rear != null ? rear.getData() : "null"));
        }
    }
    public class WhatIsATree<TE>
    {
        public class Node<TE>
        {
            public TE data;
            public Node<TE>? left, middle, right;
            public Node(TE data)
            {
                this.data = data;
                left = null; 
                middle = null;
                right = null;
            }
        }
        public Node<TE>? root;
        public Random random;
        public WhatIsATree(TE rootData)
        {
            root = new Node<TE>(rootData);
            random = new Random();
        }
        public void add(TE data)
        {
            int selector;
            Node<TE> newNode = new Node<TE>(data);
            if (root == null)
            {
                root = newNode;
                return;
            }
            if (root.left == null && root.middle == null && root.right == null)
            {
                selector = random.Next(3);
                if (selector == 0)
                {
                    root.left = newNode;
                }
                else if (selector == 1)
                {
                    root.middle = newNode;
                }
                else
                {
                    root.right = newNode;
                }
            }
            else
            {
                Node<TE>? child = root;
                while (child.left != null && child.right != null && child.middle != null)
                {
                    selector = random.Next(3);
                    if (selector == 0)
                    {
                        if (child.left != null)
                        {
                            child = child.left;
                        }
                        else
                        {
                            do
                            {
                                selector = random.Next(3);
                            }
                            while (selector != 0);
                            if (child.middle != null)
                            {
                                child = child.middle;
                            }
                            else if (child.right != null)
                            {
                                child = child.right;
                            }
                            else
                            {
                                child.left = newNode;
                                root = child;
                                return;
                            }
                        }
                    }
                    else if (selector == 1)
                    {
                        if (child.middle != null)
                        {
                            child = child.middle;
                        }
                        else
                        {
                            do
                            {
                                selector = random.Next(3);
                            }
                            while (selector != 1);
                            if (child.left != null)
                            {
                                child = child.left;
                            }
                            else if (root.right != null)
                            {
                                child = child.right;
                            }
                            else
                            {
                                child.middle = newNode;
                                root = child;
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (child.right != null)
                        {
                            child = child.right;
                        }
                        else
                        {
                            do
                            {
                                selector = random.Next(3);
                            }
                            while (selector != 2);
                            if (child.left != null)
                            {
                                child = child.left;
                            }
                            else if (child.middle != null)
                            {
                                child = child.middle;
                            }
                            else
                            {
                                child.right = newNode;
                                root = child;
                                return;
                            }
                        }
                    }
                }
                selector = random.Next(3);
                if (selector == 0)
                {
                    child.left = newNode;
                    root = child;
                }
                else if (selector == 1)
                {
                    child.middle = newNode;
                    root = child;
                }
                else
                {
                    child.right = newNode;
                    root = child;
                }
            }
        }
        public void remove(TE data)
        {
            if (root == null) return;
            int selector = random.Next(3);
            if (selector == 0)
            {
                if (root.left != null)
                {
                    if (root.left.data!.Equals(data))
                    {
                        root.left = null; // Remove the left child
                    }
                    else
                    {
                        removeHelper(root.left, data);
                    }
                }
            }
            else if (selector == 1)
            {
                if (root.middle != null)
                {
                    if (root.middle.data!.Equals(data))
                    {
                        root.middle = null; // Remove the middle child
                    }
                    else
                    {
                        removeHelper(root.middle, data);
                    }
                }
            }
            else if (selector == 2)
            {
                if (root.right != null)
                {
                    if (root.right.data!.Equals(data))
                    {
                        root.right = null; // Remove the right child
                    }
                    else
                    {
                        removeHelper(root.right, data);
                    }
                }
            }
            else
            {
                removeHelper(root, data);
            }
        }
        public bool contains(TE data)
        {
            if (root == null) return false;
            return containsHelper(root, data);
        }
        private bool containsHelper(Node<TE>? node, TE data)
        {
            if (node == null) return false;
            if (node.data!.Equals(data)) return true;
            return containsHelper(node.left, data) || containsHelper(node.middle, data) || containsHelper(node.right, data);
        }
        private void removeHelper(Node<TE>? node, TE data)
        {
            if (node == null) return;
            if (node.data!.Equals(data))
            {
                node = null; // Remove the node
                return;
            }
            removeHelper(node.left, data);
            removeHelper(node.middle, data);
            removeHelper(node.right, data);
        }
        public void printTree()
        {
            if (root == null)
            {
                Console.WriteLine("Tree is empty.");
                return;
            }
            printTreeHelper(root, 0);
        }
        private void printTreeHelper(Node<TE>? node, int level)
        {
            if (node == null) return;
            Console.WriteLine(new string(' ', level * 2) + node.data);
            printTreeHelper(node.left, level + 1);
            printTreeHelper(node.middle, level + 1);
            printTreeHelper(node.right, level + 1);
        }
    }
}