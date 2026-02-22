namespace Practice
{
    public class GenericQueue<T> // main optimization: circular buffer
    {
        T[] array;
        public int Length { get => size; }
        private int head, tail, size, AllocatedMemory;

        public GenericQueue() { }

        public void Enqueue(T item)
        {
            if(AllocatedMemory == size) 
            {
                T[] newArray = new T[AllocatedMemory == 0 ? 4 : AllocatedMemory * 2]; // resize
                for(int i = 0; i < size; ++i) newArray[i] = array[(head + i) % AllocatedMemory];
                AllocatedMemory = newArray.Length;
                array = newArray;
                head = 0;
                tail = size;
            }
            array[tail] = item;
            tail = (tail + 1) % AllocatedMemory;
            size++;
        }

        public T Dequeue()
        {
            if(this.Length == 0) throw new InvalidOperationException("Queue is empty"); 
            T result = array[head];
            head = (head + 1) % AllocatedMemory;
            size--;
            return result;
        }

        public bool Contains(T item)
        {
            for(int i = 0; i < size; ++i)
            {
                if(EqualityComparer<T>.Default.Equals(array[(head + i) % AllocatedMemory], item))
                {
                    return true;
                }
            }
            return false;
        }

        public T Peek()
        {
            if(this.size == 0) throw new InvalidOperationException("Queue is empty");
            return array[head];
        }

        public void Clear()
        {
            head = tail = size = 0;
            array = Array.Empty<T>();
            AllocatedMemory = 0;
        }

    }
}