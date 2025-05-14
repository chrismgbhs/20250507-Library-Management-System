using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.InteropServices;
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
            string holder = "";
            string newBookTitleHolder = "";
            string newBookAuthorHolder = "";
            string userNameHolder;
            string newUser;
            string newUserRole;
            string deletableUser;
            string deletableUserRole;
            char userNameFirstLetter = ' ';
            int userProcess;
            int bookInput = 0;
            int bookReturnIntput;
            int pendingBooksCounter;
            int pendingBooksDisplayCounter;
            int borrowedBooksDisplayCounter;
            int justBookCounter;
            int studentBorrowedBooksCounter;
            int adminProcessInput;
            bool nameIsValid = false;
            bool roleIsValid;
            bool userProcessAgain;

            List<string> studentsList = new List<string>
            {
                "Chris",
                "Yuan",
                "Eudrick",
                "Ivan",
                "Iram"
            };

            List<string> adminList = new List<string>
            {
                "Aris"
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

            List<string> librarianProcessList = new List<string>
            {
                "Add new books",
                "View all books",
                "View pending book requests",
                "Approve/decline book requests",
                "Logout"
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

            List<string> logs = new List<string>();




            Dictionary<string, List<string>> studentApprovedBooks = new Dictionary<string, List<string>>();
            Dictionary<string, Queue<string>> studentPendingBooks = new Dictionary<string, Queue<string>>();
            Dictionary<string, List<string>> studentDeclinedBooks = new Dictionary<string, List<string>>();

            foreach (string username in studentsList)
            {
                studentApprovedBooks[$"{username}"] = new List<string>(); 
                studentDeclinedBooks[$"{username}"] = new List<string>();
                studentPendingBooks[$"{username}"] = new Queue<string>();
            }

            logs.Add($"[{System.DateTime.Now}] - System started.");
            while (true)
            {
                nameIsValid = false;

                Console.Clear();

                while (!nameIsValid)
                {
                    Console.Write("Please enter your name: ");
                    userNameHolder = Console.ReadLine();

                    if (studentsList.Contains(userNameHolder, StringComparer.OrdinalIgnoreCase) || librariansList.Contains(userNameHolder, StringComparer.OrdinalIgnoreCase) || adminList.Contains(userNameHolder, StringComparer.OrdinalIgnoreCase))
                    {
                        userNameFirstLetter = userNameHolder[0];
                        userName = userNameFirstLetter.ToString().ToUpper() + userNameHolder.Substring(1).ToLower();
                        nameIsValid = true;
                    }

                    else
                    {
                        Console.WriteLine("The name is not on the list of users.");
                        logs.Add($"[{System.DateTime.Now}] - {userNameHolder} is not in the list of users.");
                    }
                }

                roleIsValid = false;

                while (!roleIsValid)
                {
                    Console.Write("Please select your role (student/librarian/admin): ");
                    roleInput = Console.ReadLine().ToLower();

                    if (roleInput == "student" && studentsList.Contains(userName, StringComparer.OrdinalIgnoreCase))
                    {
                            roleIsValid = true;
                    }

                    else if (roleInput == "librarian" && librariansList.Contains(userName, StringComparer.OrdinalIgnoreCase))
                    {
                            roleIsValid = true; 
                    }

                    else if(roleInput == "admin" && adminList.Contains(userName, StringComparer.OrdinalIgnoreCase))
                    {
                            roleIsValid = true;
                    }

                    else 
                    {
                        Console.WriteLine("The role is invalid.");
                        logs.Add($"[{System.DateTime.Now}] - {roleInput} is an invalid role.");
                    }
                }

                Console.WriteLine();
                userProcessAgain = true;
                logs.Add($"[{System.DateTime.Now}] - {userName} logged in.");

                while (userProcessAgain )
                {
                    
                    holder = "";
                    userProcess = 0;

                    Console.Clear();
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

                            Console.WriteLine();

                            while (true)
                            {
                                Console.Write("Please select the process that you want to do: ");
                                if (int.TryParse(Console.ReadLine(), out userProcess) && userProcess <= studentProcessList.Count && userProcess > 0)
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

                                    Console.WriteLine();

                                    while (true)
                                    {
                                        Console.Write("Select the book that you want to borrow: ");
                                        if (int.TryParse(Console.ReadLine(), out bookInput) && (bookInput <= booksList.Count()) && bookInput > 0)
                                        {
                                            break;
                                        }
                                    }
                                    
                                    if (studentPendingBooks[userName].Contains(booksList[bookInput - 1]))
                                    {
                                        Console.WriteLine($"You have already requested for {booksList[bookInput - 1]}.");
                                    }

                                    else if (studentApprovedBooks[userName].Contains(booksList[bookInput - 1]))
                                    {
                                        Console.WriteLine($"You have already borrowed {booksList[bookInput - 1]}.");
                                    }

                                    else
                                    {
                                        studentPendingBooks[userName].Enqueue(booksList[bookInput - 1]);
                                        Console.WriteLine();
                                        Console.WriteLine($"Borrow request for {booksList[bookInput - 1]} has been sent to the librarian.");
                                        Console.WriteLine();
                                        Console.WriteLine("UPDATED AVAILABLE BOOKS (NOTE THAT THE BOOK WITH A BORROW REQUEST WILL ONLY DISAPPEAR FROM THE LIST IF THE REQUEST HAS BEEN APPROVED.)");
                                        for (int bookCounter = 0; bookCounter < booksList.Count; bookCounter++)
                                        {
                                            Console.WriteLine($"{bookCounter + 1}. {booksList[bookCounter]}");
                                        }

                                        logs.Add($"Borrow request by {userName} for {booksList[bookInput - 1]} has been sent to the librarian. | {System.DateTime.Now}");
                                    }

                                    Console.WriteLine();

                                    while (true)
                                    {
                                        Console.Write("Do you want to go back to the main menu? (yes/no) You will logout if you enter no: ");
                                        holder = Console.ReadLine().ToLower();

                                        if (holder == "no")
                                        {
                                            userProcessAgain = false;
                                            logs.Add($"[{System.DateTime.Now}] - {userName} logged out.");
                                            break;
                                        }

                                        else if (holder == "yes")
                                        {
                                            break;
                                        }

                                    }

                                    break;

                                case 2:
                                    Console.Clear();
                                    Console.WriteLine("BORROWED BOOKS");
                                    studentBorrowedBooksCounter = 0;

                                    foreach (string books in studentApprovedBooks[userName])
                                    {
                                        studentBorrowedBooksCounter++;
                                        Console.WriteLine($"{studentBorrowedBooksCounter}. {books} | status: Approved");
                                    }


                                    foreach (string books in studentPendingBooks[userName])
                                    {
                                        studentBorrowedBooksCounter++;
                                        Console.WriteLine($"{studentBorrowedBooksCounter}. {books} | status: Pending");
                                    }

                                    foreach (string books in studentDeclinedBooks[userName])
                                    {   
                                        studentBorrowedBooksCounter++;
                                        Console.WriteLine($"{studentBorrowedBooksCounter}. {books} | status: Declined");
                                    }

                                    Console.WriteLine();

                                    while (true)
                                    {
                                        Console.Write("Do you want to go back to the main menu? (yes/no) You will logout if you enter no: ");
                                        holder = Console.ReadLine().ToLower();

                                        if (holder == "no")
                                        {
                                            userProcessAgain = false;
                                            logs.Add($"[{System.DateTime.Now}] - {userName} logged out.");
                                            break;
                                        }

                                        else if (holder == "yes")
                                        {
                                            break;
                                        }

                                    }

                                    break;

                                case 3:
                                    Console.Clear();
                                    justBookCounter = 0;
                                    Console.WriteLine("BORROWED BOOKS");
                                    foreach (string books in studentApprovedBooks[userName])
                                    {
                                        justBookCounter++;
                                        Console.WriteLine($"{justBookCounter}. {books}");
                                    }

                                    if (justBookCounter > 0)
                                    {
                                        while (true)
                                        {
                                            Console.WriteLine();
                                            Console.Write("Select a book that you want to return: ");
                                            if (int.TryParse(Console.ReadLine(), out bookReturnIntput) && bookReturnIntput <= studentApprovedBooks[userName].Count() && bookReturnIntput > 0)
                                            {
                                                break;
                                            }
                                        }

                                        booksList.Add(studentApprovedBooks[userName][bookReturnIntput - 1]);
                                        studentApprovedBooks[userName].RemoveAt(bookReturnIntput - 1);

                                        Console.WriteLine();
                                        Console.WriteLine("The book has been returned.");
                                        logs.Add($"{userName} successfully returned the book {studentApprovedBooks[userName][bookReturnIntput - 1]}. | {System.DateTime.Now}");

                                        Console.WriteLine();

                                        Console.WriteLine("UPDATED LIST OF AVAILABLE BOOKS");
                                        foreach (string book in booksList)
                                        {
                                            Console.WriteLine(book);
                                        }
                                    }
                                    
                                    Console.WriteLine();

                                    while (true)
                                    {
                                        Console.Write("Do you want to go back to the main menu? (yes/no) You will logout if you enter no: ");
                                        holder = Console.ReadLine().ToLower();

                                        if (holder == "no")
                                        {
                                            userProcessAgain = false;
                                            logs.Add($"[{System.DateTime.Now}] - {userName} logged out.");
                                            break;
                                        }

                                        else if (holder == "yes")
                                        {
                                            break;
                                        }
                                      
                                    }         
                                    break;

                                case 4:
                                    userProcessAgain = false;
                                    logs.Add($"[{System.DateTime.Now}] - {userName} logged out.");
                                    break;
                            }
                            break;

                        case "librarian":

                            for (int processCounter = 0; processCounter < librarianProcessList.Count; processCounter++)
                            {
                                Console.WriteLine($"{processCounter + 1}. {librarianProcessList[processCounter]}");
                            }

                            Console.WriteLine();

                            while (true)
                            {
                                Console.Write("Please select the process that you want to do: ");
                                if (int.TryParse(Console.ReadLine(), out userProcess) && userProcess <= librarianProcessList.Count && userProcess > 0)
                                {
                                    break;
                                }
                            }

                            switch (userProcess)
                            {
                                case 1:
                                    Console.Clear();
                                    Console.Write("Enter the title of the new book: ");
                                    newBookTitleHolder = Console.ReadLine();
                                    Console.Write("Please enter the author of the book: ");
                                    newBookAuthorHolder = Console.ReadLine();
                                    Console.WriteLine();
                                    logs.Add($"[{System.DateTime.Now}] - {userName} added {newBookTitleHolder} to the list of books.");
                                    booksList.Add($"{newBookTitleHolder} by {newBookAuthorHolder}");
                                    Console.WriteLine($"The book {newBookTitleHolder} by {newBookAuthorHolder} has been added to the library.");

                                    Console.WriteLine();

                                    Console.Write("Do you want to go back to the main menu? (yes/no) You will logout if you enter no: ");
                                    holder = Console.ReadLine().ToLower();

                                    if (holder == "no")
                                    {
                                        userProcessAgain = false;
                                        logs.Add($"[{System.DateTime.Now}] - {userName} logged out.");
                                        break;
                                    }

                                    else if (holder == "yes")
                                    {
                                        break;
                                    }

                                    break;
                                case 2:
                                    Console.Clear();
                                    Console.WriteLine("AVAILABLE BOOKS");
                                    for (int bookCounter = 0; bookCounter < booksList.Count; bookCounter++)
                                    {
                                        Console.WriteLine($"{bookCounter + 1}. {booksList[bookCounter]}");
                                    }
                                    Console.WriteLine();


                                    Console.WriteLine("BORROWED BOOKS");

                                    borrowedBooksDisplayCounter = 0;
                                    foreach (string keyvalue in studentApprovedBooks.Keys)
                                    {
                                        foreach (string books in studentApprovedBooks[keyvalue])
                                        {
                                            borrowedBooksDisplayCounter++;
                                            Console.WriteLine($"{borrowedBooksDisplayCounter}. {books} | Borrowed by {keyvalue}");
                                        }
                                    }

                                    Console.WriteLine(); 
                                    
                                    Console.Write("Do you want to go back to the main menu? (yes/no) You will logout if you enter no: ");
                                    holder = Console.ReadLine().ToLower();

                                    if (holder == "no")
                                    {
                                        userProcessAgain = false;
                                        logs.Add($"[{System.DateTime.Now}] - {userName} logged out.");
                                        break;
                                    }

                                    else if (holder == "yes")
                                    {
                                        break;
                                    }

                                    break;

                                case 3:
                                    Console.Clear();
                                    

                                        Console.WriteLine("PENDING BORROW REQUESTS");

                                    pendingBooksDisplayCounter = 0;

                                    foreach (string keyvalue in studentPendingBooks.Keys)
                                    {
                                        foreach (string books in studentPendingBooks[keyvalue])
                                        {
                                            pendingBooksDisplayCounter++;
                                            Console.WriteLine($"{pendingBooksDisplayCounter}. {books} | Requested by {keyvalue}");
                                        }
                                    }

                                    Console.WriteLine();

                                    Console.Write("Do you want to go back to the main menu? (yes/no) You will logout if you enter no: ");
                                    holder = Console.ReadLine().ToLower();

                                    if (holder == "no")
                                    {
                                        userProcessAgain = false;
                                        logs.Add($"[{System.DateTime.Now}] - {userName} logged out.");
                                        break;
                                    }

                                    else if (holder == "yes")
                                    {
                                        break;
                                    }

                                    break;

                                case 4:
                                    Console.Clear();
                                    pendingBooksCounter = 0;
                                    
                                    holder = "";

                                        Console.WriteLine("APPROVE/DECLINE BORROW REQUESTS");


                                    foreach (string keyvalue in studentPendingBooks.Keys)
                                    {
                                        foreach (string books in studentPendingBooks[keyvalue])
                                        {
                                            pendingBooksCounter++;
                                            Console.WriteLine($"{pendingBooksCounter}. {books} | Requested by {keyvalue}");
                                        }
                                    }

                                    Console.WriteLine();

                                    if (pendingBooksCounter > 0)
                                    {
                                        while (true)
                                        {
                                            Console.Write("Select the book that you want to approve/decline: ");
                                            if (int.TryParse(Console.ReadLine(), out bookInput) && bookInput <= pendingBooksCounter && bookInput > 0)
                                            {
                                                break;
                                            }
                                        }

                                        while (true)
                                        {
                                            Console.Write("Do you want to approve or decline the request? (approve/decline): ");
                                            holder = Console.ReadLine().ToLower();

                                            if (holder == "approve" || holder == "decline")
                                            {
                                                break;
                                            }
                                        }
                                    }
                                   
                                    Console.WriteLine();
                                    pendingBooksCounter = 0;

                                    foreach (string keyvalue in studentPendingBooks.Keys)
                                    {
                                        foreach (string books in studentPendingBooks[keyvalue])
                                        {
                                            pendingBooksCounter++;
                                            if (pendingBooksCounter == bookInput)
                                            {
                                                if (holder == "approve")
                                                {

                                                    if (booksList.Contains(books))
                                                    {
                                                        logs.Add($"[{System.DateTime.Now}] - {userName} approved {books} to be borrowed.");
                                                        studentApprovedBooks[keyvalue].Add(books);
                                                        studentPendingBooks[keyvalue].Dequeue();
                                                        booksList.Remove(books);
                                                        Console.WriteLine($"The request for {books} has been approved.");
                                                        break;
                                                    }

                                                    else
                                                    {
                                                        Console.WriteLine($"{books} is still being borrowed by someone. Please try again later.");
                                                    }
                                                    
                                                }

                                                else if (holder == "decline")
                                                {
                                                    logs.Add($"[{System.DateTime.Now}] - {userName} declined {books} to be borrowed.");
                                                    studentDeclinedBooks[keyvalue].Add(books);
                                                    studentPendingBooks[keyvalue].Dequeue();
                                                    Console.WriteLine($"The request for {books} has been declined.");
                                                    break;
                                                }
                                            }
                                        }
                                    }

                                    Console.WriteLine();

                                    Console.Write("Do you want to go back to the main menu? (yes/no) You will logout if you enter no: ");
                                    holder = Console.ReadLine().ToLower();

                                    if (holder == "no")
                                    {
                                        userProcessAgain = false;
                                        logs.Add($"[{System.DateTime.Now}] - {userName} logged out.");
                                        break;
                                    }

                                    else if (holder == "yes")
                                    {
                                        break;
                                    }

                                    break;

                                case 5:
                                    userProcessAgain = false;
                                    logs.Add($"[{System.DateTime.Now}] - {userName} logged out.");
                                    break;
                            }

                            break;

                        case "admin":
                            Console.WriteLine("1. Add user\n2. Delete user\n3. View system logs\n4. Log out");

                            while (true) 
                            {
                                Console.Write("Please select the process that you want to do: ");
                                if (int.TryParse(Console.ReadLine(), out adminProcessInput) && adminProcessInput <= 5 && adminProcessInput > 0)
                                {
                                    break;
                                }
                            }

                            switch (adminProcessInput)
                            {
                                case 1:
                                    Console.Clear();
                                    newUser = "";
                                    newUserRole = "";

                                    Console.Write("Please enter the name of the user that you want to add: ");
                                    newUser = Console.ReadLine();
                                    newUser = newUser[0].ToString().ToUpper() + newUser.Substring(1).ToLower();

                                    Console.WriteLine();

                                    while (true)
                                    {
                                        Console.Write("Please type the role of the new user (student/librarian): ");
                                        newUserRole = Console.ReadLine().ToLower();

                                        if (newUserRole == "librarian" || newUserRole == "student")
                                        {
                                            break;
                                        }
                                    }

                                    Console.WriteLine();

                                    if (newUserRole == "librarian")
                                    {
                                        librariansList.Add(newUser);
                                    }

                                    else
                                    {
                                        studentsList.Add(newUser);
                                    }

                                    Console.WriteLine();

                                    Console.WriteLine("The user has been added.");
                                    logs.Add($"{newUser} has been successfully added to the list of users as a {newUserRole}. | {System.DateTime.Now}");

                                    Console.WriteLine();

                                    while (true)
                                    {
                                        Console.Write("Do you want to go back to the main menu? (yes/no) You will logout if you enter no: ");
                                        holder = Console.ReadLine().ToLower();

                                        if (holder == "no")
                                        {
                                            userProcessAgain = false;
                                            logs.Add($"[{System.DateTime.Now}] - {userName} logged out.");
                                            break;
                                        }

                                        else if (holder == "yes")
                                        {
                                            break;
                                        }

                                    }

                                    break;

                                case 2:
                                    Console.Clear();
                                    deletableUser = "";
                                    deletableUserRole = "";

                                    Console.Write("Please enter the name of the user that you want to delete: ");
                                    deletableUser = Console.ReadLine();
                                    deletableUser = deletableUser[0].ToString().ToUpper() + deletableUser.Substring(1).ToLower();

                                    Console.WriteLine();

                                    while (true)
                                    {
                                        Console.Write("Please type the role of the user (student/librarian): ");
                                        deletableUserRole = Console.ReadLine().ToLower();

                                        if (deletableUserRole == "librarian" || deletableUserRole == "student")
                                        {
                                            break;
                                        }
                                    }

                                    Console.WriteLine();

                                    if (deletableUserRole == "librarian")
                                    {
                                        if (librariansList.Contains(deletableUser))
                                        {
                                            librariansList.Remove(deletableUser);
                                            Console.WriteLine("User successfully removed.");
                                            logs.Add($"{deletableUser} was removed by {userName}. | {System.DateTime.Now}");
                                        }

                                        else
                                        {
                                            Console.WriteLine("User not found.");
                                        }
                                    }

                                    else
                                    {
                                        if (studentsList.Contains(deletableUser))
                                        {
                                            studentsList.Remove(deletableUser);
                                            Console.WriteLine("User successfully removed.");
                                            logs.Add($"{deletableUser} was removed by {userName}. | {System.DateTime.Now}");
                                        }

                                        else
                                        {
                                            Console.WriteLine("User not found.");
                                        }
                                    }

                                    Console.WriteLine();

                                    while (true)
                                    {
                                        Console.Write("Do you want to go back to the main menu? (yes/no) You will logout if you enter no: ");
                                        holder = Console.ReadLine().ToLower();

                                        if (holder == "no")
                                        {
                                            userProcessAgain = false;
                                            logs.Add($"[{System.DateTime.Now}] - {userName} logged out.");
                                            break;
                                        }

                                        else if (holder == "yes")
                                        {
                                            break;
                                        }

                                    }

                                    break;

                                case 3:
                                    Console.Clear();
                                    Console.WriteLine("SYSTEM LOGS");
                                    foreach (string log in logs) 
                                    { 
                                        Console.WriteLine(log);
                                    }

                                    Console.WriteLine();

                                    while (true)
                                    {
                                        Console.Write("Do you want to go back to the main menu? (yes/no) You will logout if you enter no: ");
                                        holder = Console.ReadLine().ToLower();

                                        if (holder == "no")
                                        {
                                            userProcessAgain = false;
                                            logs.Add($"[{System.DateTime.Now}] - {userName} logged out.");
                                            break;
                                        }

                                        else if (holder == "yes")
                                        {
                                            break;
                                        }

                                    }

                                    break;

                                case 4:
                                    userProcessAgain = false;
                                    break;

                            }

                            break;
                    }
                }   
            }
            
        }
    }
}
