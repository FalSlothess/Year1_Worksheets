using System;

namespace comp101_worksheet4
{
    class Program
    {
        public static void Main(string[] args)
        {
            Stack stack = new Stack(2);
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);

            Console.WriteLine(stack.Pop()); // 3
            Console.WriteLine(stack.Pop()); // 2
            Console.WriteLine(stack.Pop()); // 1
            Console.WriteLine(stack.Pop()); // -1 (empty)

            Console.WriteLine("Queue: ");
            Queue queue = new Queue(2);
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);

            Console.WriteLine(queue.Dequeue()); // 1
            Console.WriteLine(queue.Dequeue()); // 2
            Console.WriteLine(queue.Dequeue()); // 3
            Console.WriteLine(queue.Dequeue()); // -1 (empty)
        }
    }

    public class Stack
    {
        private int[] items; // Array to store stack elements
        private int top; // Tracks the top element index
        private int capacity; // Capacity of the stack

        public const int ERROR_VALUE = -1;

        public Stack(int initialSize)
        {
            capacity = initialSize;
            items = new int[capacity];
            top = -1; // Stack is empty initially
        }

        public void Push(int item)
        {
            if (top + 1 == capacity) // If stack is full
            {
                EnsureCapacity(capacity * 2); // Double the capacity
            }
            items[++top] = item; // Add item and update top
        }

        public int Pop()
        {
            if (IsEmpty())
            {
                return ERROR_VALUE; // Return error if stack is empty
            }
            return items[top--]; // Return top item and decrease top
        }

        public int Peek()
        {
            if (IsEmpty())
            {
                return ERROR_VALUE; // Return error if stack is empty
            }
            return items[top]; // Return the top item without removing it
        }

        public bool IsEmpty()
        {
            return top == -1; // True if no elements in stack
        }

        public int GetSize()
        {
            return top + 1; // Number of items in stack
        }

        private void EnsureCapacity(int newCapacity)
        {
            int[] newItems = new int[newCapacity];
            for (int i = 0; i <= top; i++)
            {
                newItems[i] = items[i]; // Copy old items into new array
            }
            items = newItems; // Replace old array
            capacity = newCapacity; // Update capacity
        }
    }

    public class Queue
    {
        private int[] items; // Array to store queue elements
        private int front; // Index of the front element
        private int rear; // Index of the next position to insert
        private int size; // Current size of the queue
        private int capacity; // Capacity of the queue

        public const int ERROR_VALUE = -1;

        public Queue(int capacity)
        {
            this.capacity = capacity;
            items = new int[capacity];
            front = 0;
            rear = 0;
            size = 0;
        }

        public void Enqueue(int item)
        {
            if (size == capacity) // If queue is full
            {
                EnsureCapacity(capacity * 2); // Double the capacity
            }
            items[rear] = item; // Add item at rear position
            rear = (rear + 1) % capacity; // Circular increment
            size++;
        }

        public int Dequeue()
        {
            if (IsEmpty())
            {
                return ERROR_VALUE; // Return error if queue is empty
            }
            int result = items[front]; // Get front item
            front = (front + 1) % capacity; // Circular increment
            size--;
            return result;
        }

        public int Front()
        {
            if (IsEmpty())
            {
                return ERROR_VALUE; // Return error if queue is empty
            }
            return items[front]; // Return front item without removing it
        }

        public bool IsEmpty()
        {
            return size == 0; // Checks the queue is empty
        }

        public int GetSize()
        {
            return size; // Return current size of queue
        }

        private void EnsureCapacity(int newCapacity)
        {
            int[] newItems = new int[newCapacity];
            for (int i = 0; i < size; i++)
            {
                newItems[i] = items[(front + i) % capacity]; // Copy items in order
            }
            items = newItems; // Replace old array
            front = 0;
            rear = size; // Update rear to the end of the copied items
            capacity = newCapacity; // Update capacity
        }
    }
}
