using System;

namespace Data_Structure
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }

    public class LWLinkedList<E>
    {
        public class TheNode<E>
        {
            public E theData;
            public TheNode<E>? next, previous;

            public TheNode(E data)
            {
                theData = data;
                next = null;
                previous = null;
            }

            public E getData()
            {
                return theData;
            }
        }

        public TheNode<E>? head, tail;
        int size;

        public LWLinkedList()
        {
            head = null;
            tail = null;
            size = 0;
        }

        public void add(E data)
        {
            TheNode<E> newNode = new TheNode<E>(data);
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
        public void remove(E data)
        {
            TheNode<E>? current = head;
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
        public bool contains(E data)
        {
            TheNode<E>? current = head;
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
            TheNode<E>? current = head;
            while (current != null)
            {
                Console.Write(current.theData + " ");
                current = current.next;
            }
            Console.WriteLine();
        }
        public void replace(E oldData, E newData)
        {
            TheNode<E>? current = head;
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
        public E? getFirst()
        {
            return head!.getData();
        }
        public E? getLast()
        {
            return tail!.getData();
        }
        public E? get(int index)
        {
            if (index < 0 || index >= size)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index out of bounds");
            }
            TheNode<E>? current = head;
            for (int i = 0; i < index; i++)
            {
                current = current!.next;
            }
            return current!.getData();
        }
        public int search(E item)
        {
            TheNode<E>? current = head;
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
        public void insert(E item, int loc)
        {
            if (loc < 0 || loc > size)
            {
                throw new ArgumentOutOfRangeException(nameof(loc), "Index out of bounds");
            }
            TheNode<E> newNode = new TheNode<E>(item);
            if (loc == 0)
            {
                newNode.next = head;
                if (head != null)
                {
                    head.previous = newNode;
                }
                head = newNode;
                if (tail == null)
                {
                    tail = newNode; // If the list was empty, set tail to the new node
                }
            }
            else if (loc == size)
            {
                tail!.next = newNode;
                newNode.previous = tail;
                tail = newNode;
            }
            else
            {
                TheNode<E>? current = head;
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
    public class MWArrayList<E>
    {
        private int size, capacity;
        private E[] elements;

        public MWArrayList(int initialCapacity = 10)
        {
            capacity = initialCapacity;
            size = 0;
            elements = new E[capacity];
        }
        public void resize()
        {
            capacity *= 2;
            E[] newElements = new E[capacity];
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
        public void add(E item)
        {
            if (getSize() == getCapacity())
            {
                resize();
            }
            elements[size] = item;
            size++;
        }
        public void remove(E item)
        {
            int index = search(item);
            if (index != -1)
            {
                for (int i = index; i < size - 1; i++)
                {
                    elements[i] = elements[i + 1];
                }
                elements[size - 1] = default(E)!; // Clear the last element
                size--;
            }
        }
        public int search(E item)
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
        public E get(int index)
        {
            if (index < 0 || index >= size)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index out of bounds");
            }
            return elements[index]!;
        }
        public void replace(E oldItem, E newItem)
        {
            int index = search(oldItem);
            if (index != -1)
            {
                elements[index] = newItem;
            }
        }
        public bool contains(E item)
        {
            return search(item) != -1;
        }
        public void clear()
        {
            size = 0;
            elements = new E[capacity]; // Reset the array to its initial capacity
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
    public class Stack<E>
    {
        int size;
        MWArrayList<E> list;
        public int getSize()
        {
            return size;
        }
        public Stack()
        {
            size = -1;
            list = new MWArrayList<E>();
        }
        public void push(E item)
        {
            if (isFull())
            {
                list.resize(); // Resize the list if it is full
            }
            list.add(item);
            size++;
        }
        public E pop()
        {
            if (isEmpty())
            {
                throw new InvalidOperationException("Stack is empty");
            }
            E item = list.get(size);
            list.remove(item);
            size--;
            return item;
        }
        public bool isEmpty()
        {
            return size == -1;
        }
        public E peek()
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
        public bool contains(E item)
        {
            return list.contains(item);
        }
        public bool isFull()
        {
            return size == list.getCapacity() - 1; // Assuming the stack is full when capacity equals the last index of the list
        }
    }
    public class Queue<E>
    {
        private int size;
        private LWLinkedList<E>? list;
        private LWLinkedList<E>.TheNode<E>? front, rear;
        public Queue()
        {
            size = 0;
            list = new LWLinkedList<E>();
            front = null;
            rear = null;
        }
        public void offer(E item)
        {
            list!.add(item);
            if (front == null)
            {
                front = list.head;
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
                LWLinkedList<E>.TheNode<E>? newNode = front;
                while (newNode!.next != rear)
                {
                    if (newNode == rear!.previous)
                    {
                        rear!.previous = newNode;
                        newNode!.next = rear;
                        break;
                    }
                    else
                    {
                        newNode = newNode.next;
                    }
                }
                rear = list.tail;
                newNode = newNode!.next;
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
        public E take()
        {
            E backup = front!.getData();
            list!.remove(front!.getData()!);
            front = front.next;
            size--;
            return backup;
        }
        public E element()
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
    public class WhatIsATree<E>
    {
        public class Node<E>
        {
            public E data;
            public Node<E>? left, middle, right;
            public Node(E data)
            {
                this.data = data;
                left = null; 
                middle = null;
                right = null;
            }
        }
        public Node<E>? root;
        public Random random;
        public WhatIsATree(E rootData)
        {
            root = new Node<E>(rootData);
            random = new Random();
        }
        public void add(E data)
        {
            int selector;
            Node<E>? newNode = new Node<E>(data);
            if (root == null)
            {
                root = newNode;
                return;
            }
            else if (root.left == null && root.middle == null && root.right == null)
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
                return;
            }
            else
            {
                Node<E>? child = root;
                while (child!.left != null && child!.right != null && child!.middle != null)
                {
                    selector = random.Next(3);
                    if (selector == 0)
                    {
                        if (child!.left != null)
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
                            if (selector == 1 && child.middle != null)
                            {
                                child = child.middle;
                            }
                            else if (selector == 2 && child.right != null)
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
                        if (child!.middle != null)
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
                            if (selector == 0 && child.left != null)
                            {
                                child = child.left;
                            }
                            else if (selector == 2 && root.right != null)
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
                        if (child!.right != null)
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
                            if (selector == 0 && child.left != null)
                            {
                                child = child.left;
                            }
                            else if (selector == 1 && child.middle != null)
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
        public void remove(E data)
        {
            if (root == null) return;
            int selector = random.Next(3);
            if (selector == 0)
            {
                if (root.left == null)
                {
                    return;
                }
                else
                {
                    if (root.left.data!.Equals(data))
                    {
                        root.left = null; // Remove the left child
                        return;
                    }
                    else
                    {
                        removeHelper(root.left, data);
                    }
                }
            }
            else if (selector == 1)
            {
                if (root.middle == null)
                {
                    return;
                }
                else
                {
                    if (root.middle.data!.Equals(data))
                    {
                        root.middle = null; // Remove the middle child
                        return;
                    }
                    else
                    {
                        removeHelper(root.middle, data);
                    }
                }
            }
            else if (selector == 2)
            {
                if (root.right == null)
                {
                    return;
                }
                else
                {
                    if (root.right.data!.Equals(data))
                    {
                        root.right = null; // Remove the right child
                        return;
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
        public bool contains(E data)
        {
            if (root == null) return false;
            return containsHelper(root, data);
        }
        private bool containsHelper(Node<E>? node, E data)
        {
            if (node == null) return false;
            if (node.data!.Equals(data)) return true;
            return containsHelper(node.left, data) || containsHelper(node.middle, data) || containsHelper(node.right, data);
        }
        private void removeHelper(Node<E>? node, E data)
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
        private void printTreeHelper(Node<E>? node, int level)
        {
            if (node == null) return;
            Console.WriteLine(new string(' ', level * 2) + node.data);
            printTreeHelper(node.left, level + 1);
            printTreeHelper(node.middle, level + 1);
            printTreeHelper(node.right, level + 1);
        }
    }
}
