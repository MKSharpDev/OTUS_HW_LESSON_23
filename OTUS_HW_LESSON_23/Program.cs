//Описание / Пошаговая инструкция выполнения домашнего задания:
//Прочитать 3 файла параллельно и вычислить количество пробелов в них (через Task).
//Написать функцию, принимающую в качестве аргумента путь к папке.
//Из этой папки параллельно прочитать все файлы и вычислить количество пробелов в них.
//Замерьте время выполнения кода (класс Stopwatch).
//7 баллов: за выполненное 1 - е задание
//+ 2 балла: за выполненное 2 - е задание
//+ 1 балл: за выполненное 3 - е задание
//Минимальное количество баллов: 7



using System.Diagnostics.Metrics;

string toSearch1 = " s s ds qqdфвв sda   qdd   ";
string toSearch2 = " s s ds q  qdsda   qdd";
string toSearch3 = " s s ds qqds  da   qdd";
List<string> searchList = new List<string>() { toSearch1 , toSearch2, toSearch3 };

//Parallel.ForEach(searchList, )

int result = await searcher(toSearch1);

Console.WriteLine(result);
Console.ReadKey();

async Task<int> searcher(string toSearch)
{
    int counter = 0;
    for (int i = 0; i < toSearch.Length; i++)
    {
        if (toSearch[i] == ' ')
        {
            counter++;
        }
    }

    return counter;
}