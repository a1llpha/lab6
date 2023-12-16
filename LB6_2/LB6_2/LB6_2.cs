using System;
using System.Collections.Generic;


public delegate bool Criteria<T>(T item);


public class Repository<T>
{
    private List<T> items = new List<T>();


    public void Add(T item)
    {
        items.Add(item);
    }

   
    public List<T> Find(Criteria<T> criteria)
    {
        List<T> result = new List<T>();

        foreach (T item in items)
        {
            if (criteria(item))
            {
                result.Add(item);
            }
        }

        return result;
    }
}

class Program
{
    static void Main()
    {
      
        Repository<int> intRepository = new Repository<int>();
        intRepository.Add(1);
        intRepository.Add(2);
        intRepository.Add(3);
        intRepository.Add(4);

        Criteria<int> evenCriteria = x => x % 2 == 0;

        List<int> evenNumbers = intRepository.Find(evenCriteria);

        Console.WriteLine("Even numbers:");
        foreach (int number in evenNumbers)
        {
            Console.WriteLine(number);
        }
    }
}
