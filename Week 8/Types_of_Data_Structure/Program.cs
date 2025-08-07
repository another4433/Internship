using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Types_of_Data_Structure
{
    internal class Program
    {
        static void Main(string[] args)
        {
            KWArrayList<string> arrayList = new KWArrayList<string>();
            arrayList.addLast("Hello");
            arrayList.addLast("World");
            arrayList.addFirst("Greetings");
            arrayList.addFirst("C#");
            arrayList.remove("Greetings");
            arrayList.removeFirst();
            arrayList.display();
            Console.WriteLine($"Is the list sorted? {arrayList.isSorted()}");
            ArrayStack<string> arrayStack = new ArrayStack<string>();
            arrayStack.push("Hello");
            arrayStack.push("World");
            arrayStack.push("Greetings");
            arrayStack.push("C#");
            arrayStack.pop();
            arrayStack.pop();
            Console.WriteLine($"Top of the stack: {arrayStack.peek()}");
            arrayStack.display();
            KWLinkedList<string> linkedList = new KWLinkedList<string>();
            linkedList.addLast("Hello");
            linkedList.addLast("World");
            linkedList.addFirst("Greetings");
            Console.WriteLine($"Is the linked list sorted? {linkedList.isSorted()}");
            linkedList.display();
            LinkedStack<string> linkedStack = new LinkedStack<string>();
            linkedStack.push("Hello");
            linkedStack.push("World");
            linkedStack.push("Greetings");
            linkedStack.push("C#");
            linkedStack.pop();
            linkedStack.pop();
            Console.WriteLine($"Top of the stack: {linkedStack.peek()}");
            linkedStack.display();
            LinkedQueue<string> linkedQueue = new LinkedQueue<string>();
            linkedQueue.offer("Hello");
            linkedQueue.offer("World");
            linkedQueue.offer("Greetings");
            linkedQueue.poll();
            linkedQueue.poll();
            Console.WriteLine($"Front of the queue: {linkedQueue.element()}");
            linkedQueue.display();
        }
    }
    public class LinkedQueue<T>
    {
        internal class Node<T>
        {
            public T theData;
            public Node<T> next;
            internal Node(T data)
            {
                theData = data;
                next = null;
            }
            internal Node(T data, Node<T> next)
            {
                theData = data;
                this.next = next;
            }
            public T getData()
            {
                return theData;
            }
        }
        int size;
        Node<T> front, rear;
        public LinkedQueue()
        {
            front = null;
            rear = null;
            size = 0;
        }
        public int getSize()
        {
            return size;
        }
        public bool isEmpty()
        {
            return size == 0;
        }
        public void offer(T item)
        {
            Node<T> node = new Node<T>(item);
            if (isEmpty())
            {
                front = node;
                rear = node;
            }
            else
            {
                rear.next = node;
                rear = node;
            }
            size++;
        }
        public T poll()
        {
            if (isEmpty())
            {
                return default(T);
            }
            T backup = front.getData();
            front = front.next;
            size--;
            if (isEmpty())
            {
                rear = null;
            }
            return backup;
        }
        public T element()
        {
            if (isEmpty())
            {
                return default(T);
            }
            return front.getData();
        }
        public void display()
        {
            Node<T> node = front;
            Console.Write($"\nLinked Queue List: {node.getData()}");
            while (node.next != null)
            {
                node = node.next;
                Console.Write($", {node.getData()}");
            }
            Console.WriteLine($"\nSize: {getSize()} \n");
        }
    }
    public interface IInterface<T>
    {
        int getCapacity();
        int getSize();
        T get(int index);
        bool isEmpty();
        bool contains(T value);
        bool isFull();
        bool isRange(int index);
        int indexOf(T value);
        int lastIndexOf(T value);
        int search(T value);
        bool isEqual(T[] other);
        T set(int index, T value);
        bool replace(T oldValue, T newValue);
        void addFirst(T value);
        void addLast(T value);
        T removeFirst();
        T removeLast();
        bool removeAll();
        void clear();
        void reallocate();
        bool add(T value, int location);
        bool remove(T value);
        bool isSorted();
        void sort();
        void display();
    }
    public class KWArrayList<T> : IInterface<T>
    {
        int size = 0, capacity;
        T[] list;
        public KWArrayList(int capacity)
        {
            this.capacity = (capacity <= 0) ? 100 : capacity;
            list = new T[this.capacity];
        }
        public KWArrayList()
        {
            capacity = 100;
            list = new T[capacity];
        }
        public KWArrayList(KWArrayList<T> list)
        {
            capacity = list.capacity;
            this.list = new T[capacity];
            size = 0;
            do
            {
                this.list[size] = list.list[size];
                size++;
            }
            while (size <= list.size);
            this.list = list.list;
        }
        public int getCapacity() { return capacity; }
        public T[] getList() { return list; }
        public int getSize()
        {
            return size;
        }
        public bool isEmpty()
        {
            return size == 0;
        }
        public bool isFull()
        {
            return getSize() == getCapacity();
        }
        public int indexOf(T item)
        {
            for (int i = 0; i < size; i++)
            {
                if (((IComparable<T>)list[i]).CompareTo(item) == 0) return i;
            }
            return -1;
        }
        public int lastIndexOf(T item)
        {
            for (int i = getSize() - 1; i >= 0; i--)
            {
                if (((IComparable<T>)list[i]).CompareTo(item) == 0) return i;
            }
            return -1;
        }
        public int search(T item)
        {
            return indexOf(item);
        }
        public bool contains(T value)
        {
            return search(value) != -1;
        }
        public bool isRange(int index)
        {
            return index >= 0 && index < getSize();
        }
        public T get(int index)
        {
            if (isRange(index))
            {
                return list[index];
            }
            return default(T);
        }
        public bool isEqual(T[] other)
        {
            int count = 0;
            for (int i = 0; i <= other.Length && i <= list.Length; i++)
            {
                if (((IComparable<T>)list[i]).CompareTo(other[i]) == 0)
                {
                    count++;
                }
            }
            return count == list.Length && count == other.Length;
        }
        public T set(int index, T value)
        {
            if (isRange(index))
            {
                T backup = list[index];
                list[index] = value;
                return backup;
            }
            return default(T);
        }
        public void reallocate()
        {
            capacity *= 2;
            T[] copy = new T[capacity];
            for (int i = 0; i < getCapacity(); i++)
            {
                copy[i] = list[i];
            }
            list = copy;
        }
        public bool replace(T oldItem, T newItem)
        {
            return (((IComparable<T>)set(search(oldItem), newItem)).CompareTo(oldItem) == 0);
        }
        public void addFirst(T item)
        {
            if (isFull())
                reallocate();
            for (int i = getSize(); i > 0; i--)
            {
                list[i] = list[i - 1];
            }
            list[0] = item;
            size++;
        }
        public void addLast(T item)
        {
            if (isFull()) reallocate();
            list[size] = item;
            size++;
        }
        public bool add(T item, int location)
        {
            if (isRange(location))
            {
                if (isFull())
                {
                    reallocate();
                }
                for (int i = getSize(); i > location; i--)
                {
                    list[i] = list[i - 1];
                }
                list[location] = item;
                size++;
                return true;
            }
            return false;
        }
        public bool remove(T item)
        {
            if (isEmpty()) return false;
            else if (contains(item))
            {
                int location = lastIndexOf(item);
                for (int i = location + 1; i < getSize(); i++)
                {
                    list[i - 1] = list[i];
                }
                size--;
                return true;
            }
            else
            {
                return false;
            }
        }
        public T removeFirst()
        {
            if (isEmpty())
            {
                return default(T);
            }
            T backup = list[0];
            for (int i = 1; i < getSize(); i++)
            {
                list[i - 1] = list[i];
            }
            size--;
            return backup;
        }
        public T removeLast()
        {
            if (isEmpty())
            {
                return default(T);
            }
            T backup = list[--size];
            size--;
            return backup;
        }
        public bool removeAll()
        {
            clear();
            return isEmpty();
        }
        public void clear()
        {
            do
            {
                removeFirst();
            }
            while (!isEmpty());
        }
        public bool isSorted()
        {
            for (int i = 0; i < (size - 1) - i; i++)
            {
                for (int j = i + 1; j < size; j++)
                {
                    if (((IComparable<T>)getList()[i]).CompareTo(getList()[j]) > 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public void sort()
        {
            if (!isSorted())
            {
                bool flag = false;
                for (int i = 0; i < (getSize() - 1) - i; i++)
                {
                    for (int j = 1; j < size; j++)
                    {
                        if (((IComparable<T>)getList()[j - 1]).CompareTo(getList()[j]) > 0)
                        {
                            flag = true;
                            T backup = getList()[j - 1];
                            replace(getList()[j - 1], getList()[j]);
                            replace(getList()[j], backup);
                        }
                    }
                    if (!flag)
                    {
                        break;
                    }
                }
            }
        }
        public void display()
        {
            Console.Write($"\nList: {get(0)}");
            if (!isEmpty())
            {
                for (int i = 1; i < getSize(); i++)
                {
                    Console.Write($", {get(i)}");
                }
            }
            Console.WriteLine($"\nCapacity: {getCapacity()} \nSize: {getSize()} \n");
        }
    }
    public class ArrayStack<T>
    {
        int topOfStack;
        T[] list;
        public ArrayStack()
        {
            topOfStack = 0;
            list = new T[100];
        }
        public ArrayStack(int topOfStack)
        {
            this.topOfStack = 0;
            list = topOfStack <= 0 ? new T[100] : new T[topOfStack];
        }
        public T push(T item)
        {
            if (topOfStack == list.Length - 1)
            {
                T[] backup = new T[list.Length * 2];
                for (int i = 0; i < list.Length; i++)
                {
                    backup[i] = list[i];
                }
                list = backup;
            }
            topOfStack++;
            list[topOfStack] = item;
            return item;
        }
        public T pop()
        {
            if (isEmpty())
            {
                return default;
            }
            T backup = list[topOfStack];
            topOfStack--;
            return backup;
        }
        public bool isEmpty()
        {
            return topOfStack < 0;
        }
        public T peek()
        {
            if (isEmpty())
            {
                return default;
            }
            else { return list[topOfStack]; }
        }
        public void display()
        {
            ArrayStack<T> stack = new ArrayStack<T>();
            while (!isEmpty())
            {
                stack.push(pop());
            }
            Console.Write($"\nArray Stack List: {stack.peek()}");
            push(stack.pop());
            while (!stack.isEmpty())
            {
                push(stack.peek());
                Console.Write($", {stack.pop()}");
            }
            Console.WriteLine($"\nTop of the stack pointer: {topOfStack - 1} \n");
        }
    }
    public class KWLinkedList<T> : IInterface<T>
    {
        protected class Node<T>
        {
            internal Node<T> previous, next;
            internal T data;
            internal Node(T item)
            {
                previous = default;
                data = item;
                next = default;
            }
            internal Node(T item, Node<T> node)
            {
                previous = default;
                next = node;
                data = item;
            }
            internal Node(Node<T> previous, T item)
            {
                this.previous = previous;
                data = item;
                next = default;
            }
            internal Node(Node<T> prev, T item, Node<T> next)
            {
                previous = prev;
                this.next = next;
                data = item;
            }
            internal T getData() { return data; }
        }
        Node<T> head, tail;
        int size;
        public KWLinkedList()
        {
            head = default;
            tail = default;
            size = 0;
        }
        public int getSize()
        {
            return size;
        }
        public bool isEmpty()
        {
            return size == 0;
        }
        public bool isFull()
        {
            return head == tail;
        }
        public bool isRange(int index)
        {
            return index >= 0 && index < getSize();
        }
        public int getCapacity()
        {
            return -1;
        }
        public int indexOf(T item)
        {
            Node<T> node = head;
            for (int i = 0; i < size && node.next != default; i++)
            {
                if (((IComparable<T>)node.getData()).CompareTo(item) == 0)
                {
                    return i;
                }
                node = node.next;
            }
            return -1;
        }
        public int lastIndexOf(T item)
        {
            Node<T> node = tail;
            for (int i = getSize() - 1; i >= 0 && node.previous != default; i--)
            {
                if (((IComparable<T>)node.getData()).CompareTo(item) == 0)
                {
                    return i;
                }
                node = node.previous;
            }
            return -1;
        }
        public int search(T item)
        {
            return indexOf(item);
        }
        public bool contains(T item)
        {
            return search(item) != -1;
        }
        public T get(int index)
        {
            if (isRange(index))
            {
                Node<T> node = head;
                for (int i = 0; i < getSize() && node.next != default; i++)
                {
                    if (i == index)
                    {
                        return node.getData();
                    }
                    node = node.next;
                }
            }
            return default(T);
        }
        public void addLast(T item)
        {
            Node<T> node;
            if (isEmpty())
            {
                node = new Node<T>(item);
                head = node;
                tail = node;
            }
            else
            {
                node = new Node<T>(tail, item);
                tail.next = node;
                tail = node;
            }
            size++;
        }
        public void addFirst(T item)
        {
            Node<T> node;
            if (isEmpty())
            {
                node = new Node<T>(item);
                head = node;
                tail = node;
            }
            else
            {
                node = new Node<T>(item, head);
                head.previous = node;
                head = node;
            }
            size++;
        }
        public bool add(T item, int location)
        {
            Node<T> node;
            if (isRange(location))
            {
                if (isEmpty())
                {
                    node = new Node<T>(item);
                    head = node;
                    tail = node;
                }
                else
                {
                    Node<T> node2 = head, node3 = tail;
                    for (int i = 0; i < location && node2.next != default; i++)
                    {
                        node2 = node2.next;
                    }
                    for (int i = getSize() - 1; i > location && node3.previous != default; i--)
                    {
                        node3 = node3.previous;
                    }
                    node = new Node<T>(node2, item, node3);
                    node2.next = node;
                    node3.previous = node;
                }
                size++;
                return true;
            }
            return false;
        }
        public T removeFirst()
        {
            if (isEmpty())
            {
                return default(T);
            }
            else if (isFull())
            {
                T backup = head.getData();
                head = default;
                size--;
                return backup;
            }
            else
            {
                T backup = head.getData();
                head = head.next;
                size--;
                return backup;
            }
        }
        public T removeLast()
        {
            if (isEmpty())
            {
                return default(T);
            }
            else if (isFull())
            {
                T backup = head.getData();
                tail = default;
                size--;
                return backup;
            }
            else
            {
                T backup = tail.getData();
                tail = tail.previous;
                size--;
                return backup;
            }
        }
        public bool remove(T item)
        {
            if (isEmpty())
            {
                return false;
            }
            else if (isFull())
            {
                if (contains(item))
                {
                    head = default;
                    size--;
                    return true;
                }
                else { return false; }
            }
            else
            {
                if (contains(item))
                {
                    int location = lastIndexOf(item);
                    Node<T> node1 = tail, node2 = node1.previous;
                    for (int i = getSize() - 1; i > location && node1.previous != default && node2.previous != default; i--)
                    {
                        node1 = node1.previous;
                        node2 = node2.previous;
                    }
                    node1.previous = node2.previous;
                    size--;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public T set(int location, T newItem)
        {
            if (isRange(location))
            {
                Node<T> node = head;
                for (int i = 0; i <= location && node.next != default; i++)
                {
                    node = node.next;
                }
                T backup = node.getData();
                node.data = newItem;
                return backup;
            }
            return default(T);
        }
        public bool replace(T oldItem, T newItem)
        {
            return (((IComparable<T>)set(search(oldItem), newItem)).CompareTo(oldItem) == 0);
        }
        public bool removeAll()
        {
            head = default;
            tail = default;
            size = 0;
            return isEmpty();
        }
        public void clear()
        {
            do
            {
                removeFirst();
            }
            while (!isEmpty());
        }
        public void reallocate()
        {
            Console.WriteLine("No need to reallocate!");
        }
        public bool isSorted()
        {
            Node<T> node1 = head, node2 = node1.next;
            while (node1 != null)
            {
                while (node2 != null)
                {
                    if (((IComparable<T>)node1.getData()).CompareTo(node2.getData()) > 0)
                    {
                        return false;
                    }
                    else
                    {
                        node2 = node2.next;
                    }
                }
                node1 = node1.next;
            }
            return true;
        }
        public void sort()
        {
            if (!isSorted())
            {
                bool flag = false;
                Node<T> node1 = head, node2 = node1.next;
                while (node1 != null)
                {
                    while (node2 != null)
                    {
                        if (((IComparable<T>)node1.getData()).CompareTo(node2.getData()) > 0)
                        {
                            T temp = (T)node1.getData();
                            replace(node1.getData(), node2.getData());
                            replace(node2.getData(), temp);
                            flag = true;
                        }
                        else
                        {
                            node2 = node2.next;
                        }
                    }
                    if (!flag)
                    {
                        break;
                    }
                    else
                    {
                        node1 = node1.next;
                    }
                }
            }
        }
        public bool isEqual(T[] values)
        {
            int count = 0;
            T[] checker = new T[getSize()];
            Node<T> node = head;
            for (int i = 0; i < getSize() && node.next != default; i++)
            {
                checker[i] = node.getData();
                node = node.next;
            }
            for (int j = 0; j < checker.Length && j < values.Length; j++)
            {
                if (((IComparable<T>)checker[j]).CompareTo(values[j]) == 0)
                {
                    count++;
                }
            }
            return count == checker.Length && count == values.Length;
        }
        public void display()
        {
            Node<T> node = head;
            Console.Write($"\nList: {node.getData()}");
            while (node.next != null)
            {
                node = node.next;
                Console.Write($", {node.getData()}");
            }
            Console.WriteLine($"\nSize: {getSize()} \nCapacity: {getCapacity()}");
            Console.WriteLine("Capacity is unavailable in linked list and is not used at all. \n");
        }
    }
    public class LinkedStack<T>
    {
        private class Node<T>
        {
            internal T data;
            internal Node<T> next;
            internal Node(T data)
            {
                this.data = data;
                next = default;
            }
            internal Node(T data, Node<T> next)
            {
                this.data = data;
                this.next = next;
            }
            internal T GetValue()
            {
                return data;
            }
        }
        Node<T> topOfStack;
        public LinkedStack()
        {
            topOfStack = default;
        }
        public bool isEmpty()
        {
            return topOfStack == null;
        }
        public void push(T data)
        {
            Node<T> node;
            if (isEmpty())
            {
                node = new Node<T>(data);
                topOfStack = node;
            }
            else
            {
                node = new Node<T>(data, topOfStack);
                topOfStack = node;
            }
        }
        public T pop()
        {
            if (isEmpty())
            {
                return default(T);
            }
            else if (topOfStack.next == null)
            {
                T backup = topOfStack.GetValue();
                topOfStack = default;
                return backup;
            }
            else
            {
                T backup = topOfStack.GetValue();
                topOfStack = topOfStack.next;
                return backup;
            }
        }
        public T peek()
        {
            if (isEmpty())
            {
                return default(T);
            }
            else
            {
                return topOfStack.GetValue();
            }
        }
        public void display()
        {
            LinkedStack<T> stack = new LinkedStack<T>();
            while (!isEmpty())
            {
                stack.push(pop());
            }
            Console.Write($"\nLinked Stack List: {stack.pop()}");
            while (!stack.isEmpty())
            {
                Console.Write($", {stack.pop()}");
            }
            Console.WriteLine("\n");
        }
    }
    internal class ArrayQueue<T>
    {
        int front, rear, size, capacity;
        T[] array;
        public ArrayQueue()
        {
            front = 0;
            rear = -1;
            size = 0;
            capacity = 100;
            array = new T[capacity];
        }
        public ArrayQueue(int initial)
        {
            capacity = (initial <= 0) ? 100 : initial;
            array = new T[capacity];
            front = 0;
            rear = -1;
            size = 0;
        }
        public bool isEmpty()
        {
            return getSize() == 0;
        }
        public int getSize()
        {
            return size;
        }
        public int getCapacity()
        {
            return capacity;
        }
        public int getFront()
        {
            return front;
        }
        public int getRear()
        {
            return rear;
        }
        public bool offer(T item)
        {
            rear = (getRear() + 1) & getCapacity();
            array[getRear()] = item;
            size++;
            return true;
        }
        public T poll()
        {
            if (isEmpty())
            {
                return default(T);
            }
            T backup = array[getFront()];
            front = (getFront() + 1) & getCapacity();
            size--;
            return backup;
        }
        public T element()
        {
            return array[getFront()];
        }
        public void display()
        {
            int pointer = getFront();
            Console.Write($"\nArray Queue list: {array[pointer]}");
            pointer = (pointer + 1) & getCapacity();
            do
            {
                Console.Write($", {array[pointer]}");
                pointer = (pointer + 1) & getCapacity();
            }
            while (pointer <= getRear());
            Console.WriteLine($"Capacity: {getCapacity()} \nSize: {getSize()} \nFront pointer as index: {getFront()} \nRear pointer as index: {getRear()}");
        }
    }
    internal class Graph
    {
        public int capacity;
        public int[][] data;
        public Graph()
        {
            capacity = 10;
            data = new int[capacity][];
        }
        public Graph(int capacity)
        {
            this.capacity = (capacity <= 0) ? 10 : capacity;
            data = new int[this.capacity][];
        }
        public int getCapacity()
        {
            return capacity;
        }
        public int get(int index1, int index2)
        {
            return data[index1][index2];
        }
        public int get(int index)
        {
            return data[index][index];
        }
        public bool isRange(int index)
        {
            return index >= 0 && index < getCapacity();
        }
        public bool connect(int row, int col)
        {
            if (isRange(row) && isRange(col))
            {
                data[row][col] = 1;
                Console.WriteLine("Connection success!");
                return true;
            }
            Console.WriteLine("Connection failed!");
            return false;
        }
        public bool disconnect(int row, int col)
        {
            if (isRange(row) && isRange(col))
            {
                data[row][col] = 0;
                Console.WriteLine("Disconnection success!");
                return true;
            }
            Console.WriteLine("Disconnection failed!");
            return false;
        }
        public void display()
        {
            Console.WriteLine("Graph in matrix: \n");
            Console.Write("  ");
            for (int a = 0; a < data.Length; a++)
            {
                Console.Write(a + " ");
            }
            Console.WriteLine();
            for (int i = 0; i < data.Length; i++)
            {
                Console.Write(i + " ");
                for (int j = 0; j < getCapacity(); j++)
                {
                    Console.Write(data[i][j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
    internal class Tree
    {
        internal class Node
        {
            internal Node left, right;
            internal int data;
            internal Node(int item)
            {
                data = item;
                left = null;
                right = null;
            }
            internal int getData()
            {
                return data;
            }
        }
        Node root;
        public Tree()
        {
            root = null;
        }
        public Tree(Node node)
        {
            root = node;
        }
        public void add(int number)
        {
            Node current = root, next = root;
            Node newNode = new Node(number);
            Random rand = new Random();
            int random;
            while (next != null)
            {
                current = next;
                random = rand.Next(0, 2);
                next = random == 1 ? next.right : next.left;
            }
            random = rand.Next();
            if (current == null)
            {
                root = newNode;
            }
            else if (random == 1)
            {
                current.right = newNode;
            }
            else
            {
                current.left = newNode;
            }
        }
        public bool isLeaf()
        {
            return root != null && (root.left == null && root.right == null);
        }
        private int getCount(Node node)
        {
            if (node == null) return 0;
            else
            {
                return 1 + getCount(node.left) + getCount(node.right);
            }
        }
        public int countElements()
        {
            return getCount(root);
        }
        private int max(int x, int y)
        {
            return Math.Max(x, y);
        }
        private int findMax(Node node)
        {
            if (node == null) return 0;
            else if (node.left == null && node.right == null)
            {
                return node.getData();
            }
            else if (node.left != null && node.right == null)
            {
                return max(findMax(node.left), node.getData());
            }
            else if (node.left == null && node.right != null)
            {
                return max(node.getData(), findMax(node.right));
            }
            else
            {
                return max(findMax(node.left), findMax(node.right));
            }
        }
        public int maxElement()
        {
            return findMax(root);
        }
        private int getCountLeaf(Node node)
        {
            if (node == null) return 0;
            else if (node.left == null && node.right == null)
            {
                return 1;
            }
            else
            {
                return getCountLeaf(node.left) + getCountLeaf(node.right);
            }
        }
        public int countLeaf()
        {
            return getCountLeaf(root);
        }
        private int twoChildren(Node node)
        {
            if (node == null || (node.left == null && node.right != null) || (node.left != null && node.right == null) || (node.left == null && node.right == null))
            {
                return 0;
            }
            else
            {
                return 1 + twoChildren(node.left) + twoChildren(node.right);
            }
        }
        public int countTwoChildren()
        {
            return twoChildren(root);
        }
        public Tree leftSubTree()
        {
            return new Tree(root.left);
        }
        public Tree rightSubTree()
        {
            return new Tree(root.right);
        }
        private int summation(Node node)
        {
            if (node == null) return 0;
            else
            {
                return node.getData() + summation(node.left) + summation(node.right);
            }
        }
        public int addAllElement()
        {
            return summation(root);
        }
        private int multiplication(Node node)
        {
            if (node == null) return 1;
            else
            {
                return node.getData() * multiplication(node.left) * multiplication(node.right);
            }
        }
        public int multiplyAllElements()
        {
            return multiplication(root);
        }
        private void preTraverse(Node node)
        {
            if (node == null) return;
            Console.Write(node.getData());
            preTraverse(node.left);
            preTraverse(node.right);
        }
        private void inTraverse(Node node)
        {
            if (node == null) return;
            inTraverse(node.left);
            Console.Write(node.getData());
            inTraverse(node.right);
        }
        private void postTraverse(Node node)
        {
            if (node == null) return;
            postTraverse(node.left);
            postTraverse(node.right);
            Console.Write(node.getData());
        }
        public void display()
        {
            Console.Write("Pre-Traverse: ");
            preTraverse(root);
            Console.WriteLine();
            Console.Write("In-Traverse: ");
            inTraverse(root);
            Console.WriteLine();
            Console.Write("Post-Traverse: ");
            postTraverse(root);
            Console.WriteLine();
        }
    }
    internal class ArrayHashing
    {
        int size;
        int[] array;
        public ArrayHashing(int capacity)
        {
            int initial = (capacity <= 0) ? 10 : capacity;
            array = new int[initial];
        }
        public ArrayHashing()
        {
            size = 0;
            array = new int[10];
        }
        public int getSize()
        {
            return size;
        }
        public int[] getArray()
        {
            return array;
        }
        public bool isEmpty()
        {
            return size == 0;
        }
        public bool isFull()
        {
            return size == array.Length;
        }
        public bool isRange(int index)
        {
            return index >= 0 && index < array.Length;
        }
        public int get(int index)
        {
            if (index % array.Length == index)
                return array[index];
            else
                return -1;
        }
        public void reallocate()
        {
            int[] backup = new int[array.Length * 2];
            for (int i = 0; i < getSize(); i++)
            {
                int location = array[i] % getSize();
                while (backup[location] != 0)
                {
                    location = (location + 1) % getSize();
                }
                if (backup[location] == 0)
                {
                    backup[location] = array[i];
                }
            }
            array = backup;
        }
        public int search(int index)
        {
            int location = index % array.Length;
            int count = 0;
            while (array[location] != index)
            {
                location = (location + 1) % array.Length;
                if (count == array.Length)
                {
                    break;
                }
                else
                {
                    count++;
                }
            }
            if (array[location] == index)
            {
                return location;
            }
            else
            {
                return -1;
            }
        }
        public void add(int value)
        {
            if (isFull())
            {
                reallocate();
            }
            int location = value % array.Length;
            int count = 0;
            while (array[location] != 0)
            {
                location = (location + 1) % array.Length;
                if (count == array.Length)
                {
                    break;
                }
                else
                {
                    count++;
                }
            }
            if (array[location] == 0)
            {
                array[location] = value;
                size++;
            }
            else
            {
                Console.WriteLine("Element not added.");
            }
        }
        public int delete(int index)
        {
            if (isEmpty())
            {
                return -1;
            }
            int location = search(index);
            int count = 0;
            while (location == -1)
            {
                location = (location + 1) % array.Length;
                if (count == array.Length)
                {
                    break;
                }
                else
                {
                    count++;
                }
            }
            if (array[location] == index)
            {
                int backup = array[location];
                array[location] = 0;
                size--;
                return backup;
            }
            else { return -1; }
        }
        public void display()
        {
            Console.Write("\nList: ");
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(get(i));
            }
            Console.WriteLine("\n");
        }
    }
    internal class LinkedHashing
    {
        internal class Node
        {
            public int data;
            public Node next;
            internal Node(int data, Node next)
            {
                this.data = data;
                this.next = next;
            }
            internal Node(int data)
            {
                this.data = data;
                next = null;
            }
            internal int getData()
            {
                return data;
            }
        }
        Node[] array;
        int capacity;
        public LinkedHashing()
        {
            capacity = 10;
            array = new Node[capacity];
        }
        public LinkedHashing(int capacity)
        {
            this.capacity = (capacity <= 0) ? 10 : capacity;
            array = new Node[capacity];
        }
        public int getCapacity()
        {
            return capacity;
        }
        public void add(int item)
        {
            int location = item % getCapacity();
            if (array[location] == null)
            {
                array[location] = new Node(item);
            }
            else
            {
                Node newNode = new Node(item, array[location]);
                array[location] = newNode;
            }
        }
        public int search(int item)
        {
            int location = item % getCapacity();
            Node temp = array[location];
            while (temp != null)
            {
                if (temp.getData() == item)
                {
                    return location;
                }
                else
                {
                    temp = temp.next;
                }
            }
            return -1;
        }
        public int remove(int item)
        {
            int location = search(item);
            Node temp1 = array[location], temp2 = array[location];
            while (temp1 != null && temp2.next != null)
            {
                temp2 = temp2.next;
                if (temp1.getData() == item)
                {
                    int backup = array[location].getData();
                    array[location] = array[location].next;
                    return backup;
                }
                else if (temp2.getData() == item)
                {
                    int backup = temp2.getData();
                    temp1.next = temp2.next;
                    return backup;
                }
                else
                {
                    temp1 = temp2;
                }
            }
            return -1;
        }
        public int get(int location)
        {
            if (location % getCapacity() == location)
            {
                Node temp = array[location];
                for (int i = 0; i <= location && temp.next != null; i++)
                {
                    temp = temp.next;
                }
                return temp.getData();
            }
            return -1;
        }
    }
}

