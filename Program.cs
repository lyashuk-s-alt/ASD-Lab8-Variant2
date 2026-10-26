using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Rozumatic_ASD_Lab8
{
    // Базовий клас стеку на основі масиву з лабораторної роботи
    public class ArrayStack<T>
    {
        protected T[] _items;
        protected int _top = -1;

        public int Count => _top + 1;
        public int Capacity => _items.Length;
        public bool IsEmpty => _top == -1;

        public ArrayStack(int capacity = 5)
        {
            _items = new T[capacity];
        }

        // Додавання елемента на вершину - O(1)
        public virtual void Push(T item)
        {
            if (_top == _items.Length - 1)
            {
                throw new StackOverflowException("Стек переповнено! Максимальна місткість вичерпана.");
            }
            _items[++_top] = item;
        }

        // Вилучення елемента з вершини - O(1)
        public virtual T Pop()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Неможливо виконати Pop: Стек порожній!");
            }
            T item = _items[_top];
            _items[_top--] = default!; // очищення для збирача сміття
            return item;
        }

        // Перегляд вершини без вилучення - O(1)
        public T Peek()
        {
            if (IsEmpty) throw new InvalidOperationException("Стек порожній!");
            return _items[_top];
        }

        public void Print()
        {
            Console.Write($"[Count={Count}/{Capacity}]: Bottom -> [ ");
            for (int i = 0; i <= _top; i++) Console.Write($"{_items[i]} ");
            Console.WriteLine("] -> TOP");
        }
    }

    // ==========================================
    // ІНДИВІДУАЛЬНЕ ЗАВДАННЯ: ВАРІАНТ 2
    // Стек із підтримкою операції GetMin() за O(1) часу
    // ==========================================
    public class MinStack : ArrayStack<int>
    {
        // Додатковий стек для зберігання поточних мінімумів
        private readonly ArrayStack<int> _minStack;

        public MinStack(int capacity = 5) : base(capacity)
        {
            _minStack = new ArrayStack<int>(capacity);
        }

        public override void Push(int item)
        {
            // Спочатку додаємо елемент у основний стек через базовий метод
            base.Push(item);

            // Якщо допоміжний стек порожній або новий елемент менший/рівний поточному мінімуму
            if (_minStack.IsEmpty || item <= _minStack.Peek())
            {
                _minStack.Push(item);
            }
            else
            {
                // Якщо новий елемент більший за мінімум, дублюємо поточний мінімум
                _minStack.Push(_minStack.Peek());
            }
        }

        public override int Pop()
        {
            // Вилучаємо елемент з обох стеків одночасно, щоб синхронізувати стани
            _minStack.Pop();
            return base.Pop();
        }

        // Повернення мінімального елемента за O(1)
        public int GetMin()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Неможливо отримати мінімум: Стек порожній!");
            }
            return _minStack.Peek();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==========================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №8: ВАРІАНТ 2 (MIN STACK - GETMIN ЗА O(1))");
            Console.WriteLine("==========================================================================");

            // Створюємо MinStack місткістю 6 елементів
            MinStack minStack = new MinStack(6);

            Console.WriteLine("\n--- 1. Додавання елементів у MinStack (Push) ---");
            
            Console.WriteLine("Додаємо: 8"); minStack.Push(8);
            Console.WriteLine($"Поточний мінімум (GetMin): {minStack.GetMin()}"); // 8

            Console.WriteLine("Додаємо: 5"); minStack.Push(5);
            Console.WriteLine($"Поточний мінімум (GetMin): {minStack.GetMin()}"); // 5

            Console.WriteLine("Додаємо: 12"); minStack.Push(12);
            Console.WriteLine($"Поточний мінімум (GetMin): {minStack.GetMin()}"); // 5 (12 не вплинуло на мінімум)

            Console.WriteLine("Додаємо: 3"); minStack.Push(3);
            Console.WriteLine($"Поточний мінімум (GetMin): {minStack.GetMin()}"); // 3 (новий мінімум)

            Console.Write("\nПоточний стан основного стеку: ");
            minStack.Print();

            Console.WriteLine("\n--- 2. Вилучення елементів (Pop) та відстеження мінімуму ---");
            
            Console.WriteLine($"Вилучено (Pop): {minStack.Pop()}"); // Видалили 3
            Console.WriteLine($"Поточний мінімум повернувся до: {minStack.GetMin()}"); // 5

            Console.WriteLine($"Вилучено (Pop): {minStack.Pop()}"); // Видалили 12
            Console.WriteLine($"Поточний мінімум все ще: {minStack.GetMin()}"); // 5

            Console.WriteLine($"Вилучено (Pop): {minStack.Pop()}"); // Видалили 5
            Console.WriteLine($"Поточний мінімум повернувся до: {minStack.GetMin()}"); // 8

            Console.WriteLine("\n✅ Роботу зі стеком MinStack завершено успішно!");
        }
    }
}
