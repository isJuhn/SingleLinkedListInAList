using System;
using System.Collections.Generic;

namespace SingleLinkedListInAList
{
    class ListInAList<T> //: IList<T>, System.Collections.IList, IReadOnlyList<T>
    {
        private Dictionary<int, Pos> pos = new Dictionary<int, Pos>();
        private Node<SimpleSingleLinkedList<T>> first;
        private int count = 0;
        private int listCount = 0;
        private float itemsPerListToNumListsRatio;

        public ListInAList()
        {
            itemsPerListToNumListsRatio = 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ratio">The ratio between items per inner list and the total number of inner lists. Default is 1</param>
        public ListInAList(float ratio)
        {
            itemsPerListToNumListsRatio = ratio;
        }

        public void Add(T item)
        {
            if (first == null)
            {
                first = new Node<SimpleSingleLinkedList<T>>(new SimpleSingleLinkedList<T>(item));
                pos.Add(0, new Pos(0, 0));
                listCount++;
            }
            else
            {
                if ((count + 1.0) / listCount / listCount > itemsPerListToNumListsRatio)
                {
                    var temp = first;
                    while (temp.Next != null)
                    {
                        temp = temp.Next;
                    }
                    temp.Next = new Node<SimpleSingleLinkedList<T>>(new SimpleSingleLinkedList<T>(item));
                    pos.Add(count, new Pos(listCount, 0));
                    listCount++;
                }
                else
                {
                    var temp = first;
                    int i = 0;
                    while (temp.Element.Count + 1 > itemsPerListToNumListsRatio * listCount)
                    {
                        temp = temp.Next;
                        i++;
                    }
                    temp.Element.Add(item);
                    pos.Add(count, new Pos(i, temp.Element.Count - 1));
                }
            }
            count++;
        }

        public T GetAt(int index)
        {
            var temp = first;
            for (int i = 0; i < pos[index].PosOuter; i++)
            {
                temp = temp.Next;
            }
            return temp.Element.GetAt(pos[index].PosInner);
        }

        public int Count { get { return count; } private set { count = value; } }

        class SimpleSingleLinkedList<E>
        {
            private Node<E> first;
            private int count = 0;

            public SimpleSingleLinkedList()
            {

            }

            public SimpleSingleLinkedList(E element)
            {
                Add(element);
            }

            public void Add(E item)
            {
                if (first == null)
                {
                    first = new Node<E>(item);
                }
                else
                {
                    Node<E> last = first;
                    while (last.Next != null)
                    {
                        last = last.Next;
                    }
                    last.Next = new Node<E>(item);
                }
                count++;
            }

            public void RemoveAt(int index)
            {
                if (first == null || index >= count)
                    throw new IndexOutOfRangeException($"index is out of range: {index} with a count of {count} items in the collection");
                Node<E> temp = first;
                for (int i = 0; i < index - 1; i++)
                {
                    temp = temp.Next;
                }
                Node<E> next = temp.Next.Next;
                temp.Next.Next = null;
                temp.Next = next;
                count--;
            }

            public E GetAt(int index)
            {
                if (first == null || index >= count)
                    throw new IndexOutOfRangeException($"index is out of range: {index} with a count of {count} items in the collection");
                Node<E> temp = first;
                for (int i = 0; i < index; i++)
                {
                    temp = temp.Next;
                }
                return temp.Element;
            }

            public int Count { get { return count; } private set { count = value; } }
        }

        class Node<E>
        {
            private E element;
            private Node<E> next;

            public Node(E e)
            {
                element = e;
            }

            public Node<E> Next { get { return next; } set { next = value; } }

            public E Element { get { return element; } set { element = value; } }
        }

        class Pos
        {
            private int posOuter;
            private int posInner;

            public Pos()
            {

            }

            public Pos(int posOuter, int posInner)
            {
                this.posOuter = posOuter;
                this.posInner = posInner;
            }

            public int PosInner
            {
                get { return posInner; }
                set { posInner = value; }
            }

            public int PosOuter
            {
                get { return posOuter; }
                set { posOuter = value; }
            }
        }
    }
}
