using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20250507_Library_Management_System
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string userName = "";
            string roleInput = "";
            int userProcess;
            int bookInput;
            int bookReturnIntput;
            bool nameIsValid = false;
            bool roleIsValid;

            List<string> studentsList = new List<string>
            {
                "Chris",
                "Yuan",
                "Eudrick",
                "Ivan",
                "Iram"
            };

            List<string> librariansList = new List<string> {
                "Rose",
                "Julian",
                "Chris",
                "Gilbert",
                "Marcy"
            };

            List<string> studentProcessList = new List<string> 
            { 
                "Borrow books",
                "View borrowed books",
                "Return books",
                "Logout",
            };

            List<string> booksList = new List<string>
            {
                "The Let Them Theory • A Life-Changing Tool That Millions of People Can’t Stop Talking About by Mel Robbins",
                "AI Engineering: Building Applications with Foundation Models by Chip Huyen",
                "The Mountain Is You: Transforming Self-Sabotage Into Self-Mastery by Brianna Wiest",
                "101 Essays That Will Change The Way You Think by Brianna Wiest",
                "LLM Engineers Handbook: Master the art of engineering large language models from concept to production by Paul Iusztin, Maxime Labonne",
                "The little prince by de Saint-Exupery Antoine, Katherine Woods"
            };

            List<string> pendingBorrowRequests = new List<string>();
            List<string> approvedBorrowRequests = new List<string>();


            Dictionary<string, List<string>> studentApprovedBooks = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> studentPendingBooks = new Dictionary<string, List<string>>();

            foreach (string username in studentsList)
            {
                studentApprovedBooks[$"{username}"] = new List<string>();
                studentPendingBooks[$"{username}"] = new List<string>();
            }

            while (true) 
            {
                nameIsValid = false;

                while (!nameIsValid) 
                {
                    Console.Write("Please enter your name: ");
                    userName = Console.ReadLine();

                    if (studentProcessList.Contains(userName) || librariansList.Contains(userName)) 
                    { 
                        nameIsValid = true;
                    }
                }

                roleIsValid = false;

                while (!roleIsValid)
                {
                    Console.Write("Please select your role (student/librarian): ");
                    roleInput = Console.ReadLine().ToLower();

                    if (roleInput == "student")
                    {
                        roleIsValid = true;
                    }
                }

                Console.WriteLine();

                switch (roleInput) 
                {
                    case "student":

                        Console.WriteLine("AVAILABLE BOOKS");
                        for (int bookCounter = 0; bookCounter < booksList.Count; bookCounter++)
                        {
                            Console.WriteLine($"{bookCounter + 1}. {booksList[bookCounter]}");
                        }

                        Console.WriteLine();

                        for (int studentProcessListCounter = 0; studentProcessListCounter < studentProcessList.Count; studentProcessListCounter++) 
                        {
                            Console.WriteLine($"{studentProcessListCounter + 1}. {studentProcessList[studentProcessListCounter]}");
                        }

                        while (true)
                        {
                            Console.Write("Please select the process that you want to do: ");
                            if (int.TryParse(Console.ReadLine(), out userProcess) && userProcess <= studentProcessList.Count && userProcess > 0 )
                            {
                                break;
                            }
                        }

                        switch (userProcess)
                        {
                            case 1:
                                Console.Clear();
                                Console.WriteLine("AVAILABLE BOOKS");

                                for (int bookCounter = 0; bookCounter < booksList.Count; bookCounter++)
                                {
                                    Console.WriteLine($"{bookCounter + 1}. {booksList[bookCounter]}");
                                }

                                while (true)
                                {
                                    Console.Write("Select the book that you want to borrow: ");
                                    if (int.TryParse(Console.ReadLine(), out bookInput) && (bookInput <= booksList.Count()) && bookInput > 0)
                                    {
                                        break;
                                    }
                                }

                                pendingBorrowRequests.Add($"{userName} | {booksList[bookInput - 1]} ");
                                studentPendingBooks[userName].Add(booksList[bookInput -1]);

                                Console.WriteLine();

                                Console.WriteLine($"Borrow request for for {booksList[bookInput - 1]} has been sent to the librarian");

                            break;

                            case 2:
                                Console.Clear();
                                Console.WriteLine("BORROWED BOOKS");

                                foreach (string books in studentApprovedBooks[userName]) 
                                {
                                    Console.Write(books + " | status: Approved");
                                    Console.WriteLine();
                                }

                                foreach (string books in studentPendingBooks[userName]) 
                                {
                                    Console.WriteLine($"{books} | status: Pending");
                                    Console.WriteLine();
                                }
                                break;

                            case 3:
                                Console.Clear();
                                Console.WriteLine("BORROWED BOOKS");
                                foreach (string books in studentApprovedBooks[userName])
                                {
                                    Console.Write(books);

                                    while (true)
                                    {
                                        Console.Write("Select a book that you want to return: ");
                                        if (int.TryParse(Console.ReadLine(), out bookReturnIntput) && bookReturnIntput < studentApprovedBooks[userName].Count() && bookReturnIntput > 0)
                                        {
                                            break;
                                        }
                                    }

                                    booksList.Add(studentApprovedBooks[userName][bookReturnIntput]);
                                    studentApprovedBooks[userName].RemoveAt(bookReturnIntput - 1);
                                }
                                break;
                        }
                        break;

                    case "librarian":
                        break;
                }
            }
            
        }
    }
}
