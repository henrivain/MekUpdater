using MekPathLibrary;


//MekUpdateProcess process = new("matikkaeditorinaantaja", "Matikkaeditorinaantaja");

string shit = Environment.GetEnvironmentVariable("USERPROFILE")?.ToString() + @"\Downloads" + Path.DirectorySeparatorChar;
string shit2 = new FolderPath(Environment.GetEnvironmentVariable("USERPROFILE")?.ToString() + @"\Downloads" + Path.DirectorySeparatorChar).ToString();

Console.WriteLine(shit2);
Console.WriteLine(new FolderPath(shit).ToString());
Console.WriteLine();


Console.WriteLine(shit);



Console.ReadKey();
