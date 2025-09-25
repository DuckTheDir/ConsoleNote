// Made By DuckTheDir (on github)
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Terminal.Gui;
Application.Init();


bool showPopups = true;
int color = 7 ;
Math.Clamp(color, 1, 15);

bool mod = false;
string? filePath = null;


string userDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
string appDir = Path.Combine(userDir, ".consolenote_something23421");
Directory.CreateDirectory(appDir);

string appPath = Path.Combine(appDir, "n.bin");

// Program.cs with top-level statements


if (args.Length > 0)
{
    filePath = args[0];
}



void saveNumbers()
{

   using (BinaryWriter writer = new BinaryWriter(File.Open(appPath, FileMode.Create)))
        {
            writer.Write7BitEncodedInt(showPopups ?1:0);
            writer.Write7BitEncodedInt(Math.Clamp(color, 1, 15));
       }
}



void checkForNuMBERS()
{
    if (File.Exists(appPath))
    {
        using (BinaryReader reader = new BinaryReader(File.OpenRead(appPath)))
        {
            bool n1 = reader.Read7BitEncodedInt() == 1;
            int n2 = Math.Clamp(reader.Read7BitEncodedInt(),1,15);
            showPopups = n1;
            color = n2;
            
        }
    }
    else
    {
        saveNumbers();
    }
}

checkForNuMBERS();



Toplevel appTop = Application.Top;

Label fileLabel = new Label() {Text = "|File-Path: null",
    ColorScheme = new ColorScheme { Normal = Application.Driver.MakeAttribute(Color.DarkGray, Color.Black) }
};
FastScrollTextView textView = new FastScrollTextView() { X = 0, Y = 3, Width = Dim.Fill(), Height = Dim.Fill() , Text = "Print here..."};

Colors.Base = new ColorScheme() {};
Colors.Menu = new ColorScheme() { Normal = Application.Driver.MakeAttribute(Color.Gray, Color.Black)};

Window win = new Window("ConsoleNote")
{
    X = 0,
    Y = 0,
    Width = Dim.Fill(),
    Height = Dim.Fill(),
    ColorScheme = new ColorScheme() { HotNormal = Application.Driver.MakeAttribute(Color.Green, Color.Black) }
};


void changeColor(int clr)
{
    int resultColor = Math.Clamp(clr, 1, 15);
    textView.ColorScheme = new ColorScheme() { Normal = Application.Driver.MakeAttribute((Color)resultColor, Color.Black) , Focus = Application.Driver.MakeAttribute((Color)resultColor, Color.Black) };
    color = resultColor;
    textView.SetNeedsDisplay();
}

changeColor(color);

MenuBar menuBar = new MenuBar(new MenuBarItem[]
{
    new MenuBarItem("[.File]",new MenuItem[] {
        new MenuItem("#New-File","Hint: makes new file.", new_minecraftUpdate),
        new MenuItem("#Open","Hint: opens file.",OpenFile),
        new MenuItem("#Save", "Hint: saves file.",save),
        new MenuItem("#Save-As", "Hint: saves file with save-place.",save_joJo_as),
        new MenuItem("#End","", end)
    }),
    new MenuBarItem("[.Config]",new MenuItem[]{ 
        new MenuItem("#Undue-Popups", "Hint: True(1): show ,False(0): hide.",ask_for_unduePopupppers),
        new MenuItem("#Color","Hint: Uses Console's color",ask_for_color),
    }),
});
menuBar.Y = 1;



win.Add(fileLabel,textView,menuBar);
appTop.Add(win);
textView.ContentsChanged += (_) =>
{
    mod = true;
    updSckpchK_name();
};


void end()
{
    Console.Clear();
    Console.ResetColor();
    Console.WriteLine("---##|[{ ConsoleNote.exe was ended. }]|###---");
    Environment.Exit(0);
    
}


Application.Run();




void OpenFile()
{
    int action = confirm();
    if (action == 2) return;
    if (action == 0) save();

    
    OpenDialog opD = new OpenDialog("File-Opener", "Text: choose file.") { AllowedFileTypes = [".txt",".json"] };
    Application.Run(opD);
    if (!opD.Canceled && opD.FilePaths.Count > 0)
    {
        if (showPopups) MessageBox.Query("File-chosen", $"You chose: {opD.FilePaths[0]}", "OK");
        filePath = opD.FilePaths[0].ToString();
        textView.Text = File.ReadAllText(filePath);
        updSckpchK_name();
    }
}

//hellllloooooooooooooooooooo world
int confirm()
{
    if (!mod) return 1;
    int n = MessageBox.Query("Unsaved Changes",
        "Save file?",
        "Yes", "No","Cancel");
    return n;
}

void ask_for_unduePopupppers()
{
    if (ask_a__numbero("Show undue?", 0, 1) == 0)
    {
        showPopups = false;
        saveNumbers();
    }
    else
    {
        showPopups = true;
        saveNumbers();
    }
}
void ask_for_color()
{
    changeColor(ask_a__numbero("Color-changer", 1, 15));
}

void new_minecraftUpdate()
{
    int action = confirm();
    if (action == 2) return;
    if (action == 0) save();
    {
        textView.Text = "";
        filePath = null;
        mod = false;
        updSckpchK_name();
    }
}

void save()
{
    if (filePath == null)
    {
        save_joJo_as();
        return;
    }
    File.WriteAllText(filePath, textView.Text.ToString());
    mod = false;
    updSckpchK_name();
    if (showPopups) MessageBox.Query("File-saved", $"Your File was saved.", "OK");
}

void save_joJo_as()
{
    var d = new SaveDialog("File-Saver", "Text: choose save-place.") { AllowedFileTypes = [".txt", ".json"] };
    Application.Run(d);
    if (!d.Canceled && !string.IsNullOrEmpty(d.FilePath.ToString()))
    {
        filePath = d.FilePath.ToString();
        save();
    }
}

void updSckpchK_name()
{
    string name = filePath ?? "null";
    if (mod) name += " *";
    fileLabel.Text = "|File-path: " + name;
}
// custom ui classes and etc. 🌚


int ask_a__numbero(string title = "null", int min = 1, int max = 10)
{
    int result = 0;
    List<int> numbers = Enumerable.Range(min, max - min + 1).ToList();


    ListView list = new ListView(numbers) { Width = Dim.Fill(), Height = numbers.Count };

    Dialog d = new Dialog(title, 20, Math.Clamp(numbers.Count*2,5,20));
    d.Add(list);

    Button ok = new Button("OK");
    ok.Clicked += () =>
    {
        if (list.SelectedItem >= 0)
        {
            var item = list.Source.ToList()[list.SelectedItem];
            if (item is int value)
                result = value;
            else
                result = 0;
        }
        Application.RequestStop();
    };

    Button cancel = new Button("Cancel");
    cancel.Clicked += () => Application.RequestStop();

    d.AddButton(ok);
    d.AddButton(cancel);

    Application.Run(d);
    return result;
}

class FastScrollTextView : TextView
{
    DateTime lastScroll = DateTime.MinValue;
    int lastDir = 0;

    public override bool OnMouseEvent(MouseEvent me)
    {
        if (me.Flags.HasFlag(MouseFlags.WheeledUp) || me.Flags.HasFlag(MouseFlags.WheeledDown))
        {
            int dir = me.Flags.HasFlag(MouseFlags.WheeledUp) ? -1 : 1;
            DateTime now = DateTime.Now;
            float ms = (now - lastScroll).Milliseconds;

            int step = 1;
            if (dir == lastDir && ms < 100) step = 5;
            else if (dir == lastDir && ms < 200) step = 3;

            int newTop = TopRow + dir * step;
            if (newTop < 0) newTop = 0;
            if (newTop > Lines - 1) newTop = Lines - 1;

            TopRow = newTop;
            SetNeedsDisplay();

            lastScroll = now;
            lastDir = dir;
            return true;
        }

        return base.OnMouseEvent(me);
    }
}


