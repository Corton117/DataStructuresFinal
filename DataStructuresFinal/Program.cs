using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

class Quest
{
    public string Name { get; set; }
    public string Location { get; set; }
    public int Reward { get; set; }
    public int TimeDependency { get; set; }

    public Quest(string name, string location, int reward, int timeDependency)
    {
        Name = name;
        Location = location;
        Reward = reward;
        TimeDependency = timeDependency;
    }
}

class QuestManager
{
    private List<Quest> quests = new List<Quest>();
    private List<string> locationsHeap = new List<string>();

    public void AddQuest(string name, string location, int reward, int timeDependency)
    {
        Quest newQuest = new Quest(name, location, reward, timeDependency);
        quests.Add(newQuest);

        // Add location to the heap if not already present
        if (!locationsHeap.Contains(location))
        {
            locationsHeap.Add(location);
            HeapifyUp(locationsHeap, locationsHeap.Count - 1);
        }
    }

    private void HeapifyUp(List<string> list, int index)
    {
        while (index > 0)
        {
            int parent = (index - 1) / 2;
            if (string.Compare(list[index], list[parent]) < 0)
                break;

            string temp = list[index];
            list[index] = list[parent];
            list[parent] = temp;
            index = parent;
        }
    }

    public string GetQuestsByLocation(string currentLocation)
    {
        string result = $"Quests at {currentLocation}:\n";
        foreach (var quest in quests.Where(q => q.Location == currentLocation))
        {
            result += $"  {quest.Name} - Reward: {quest.Reward}, Time Dependency: {quest.TimeDependency} days\n";
        }
        return result;
    }
}

public class MainForm : Form
{
    private QuestManager questManager = new QuestManager();
    private TextBox outputTextBox;
    private TextBox currentLocationTextBox;

    public MainForm()
    {
        Text = "Quest Manager";
        Size = new System.Drawing.Size(400, 350);

        // Input controls
        Label nameLabel = new Label { Text = "Quest Name:", Location = new System.Drawing.Point(10, 10), AutoSize = true };
        TextBox nameTextBox = new TextBox { Location = new System.Drawing.Point(120, 10), Width = 200 };
        Label locationLabel = new Label { Text = "Location:", Location = new System.Drawing.Point(10, 40), AutoSize = true };
        TextBox locationTextBox = new TextBox { Location = new System.Drawing.Point(120, 40), Width = 200 };
        Label rewardLabel = new Label { Text = "Reward:", Location = new System.Drawing.Point(10, 70), AutoSize = true };
        TextBox rewardTextBox = new TextBox { Location = new System.Drawing.Point(120, 70), Width = 200 };
        Label timeDependencyLabel = new Label { Text = "Time Dependency:", Location = new System.Drawing.Point(10, 100), AutoSize = true };
        TextBox timeDependencyTextBox = new TextBox { Location = new System.Drawing.Point(120, 100), Width = 200 };
        Label currentLocationLabel = new Label { Text = "Current Location:", Location = new System.Drawing.Point(10, 130), AutoSize = true };
        currentLocationTextBox = new TextBox { Location = new System.Drawing.Point(120, 130), Width = 200 };
        Button addButton = new Button { Location = new System.Drawing.Point(10, 160), Text = "Add Quest" };
        addButton.Click += (sender, e) =>
        {
            string name = nameTextBox.Text;
            string location = locationTextBox.Text;
            int reward;
            int timeDependency;

            try
            {
                reward = int.Parse(rewardTextBox.Text);
                timeDependency = int.Parse(timeDependencyTextBox.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid reward or time dependency. Please enter integer values.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            questManager.AddQuest(name, location, reward, timeDependency);
            outputTextBox.Text = "Quest added successfully!";
        };

        // Output textbox
        outputTextBox = new TextBox { Location = new System.Drawing.Point(10, 200), Width = 360, Height = 90, Multiline = true, ReadOnly = true };

        // Buttons to view quests
        Button viewByLocationButton = new Button { Location = new System.Drawing.Point(10, 300), Text = "View by Current Location" };
        viewByLocationButton.Click += (sender, e) =>
        {
            string currentLocation = currentLocationTextBox.Text;
            outputTextBox.Text = questManager.GetQuestsByLocation(currentLocation);
        };

        // Add controls to the form
        Controls.AddRange(new Control[] { nameLabel, nameTextBox, locationLabel, locationTextBox, rewardLabel, rewardTextBox, timeDependencyLabel, timeDependencyTextBox, currentLocationLabel, currentLocationTextBox, addButton, outputTextBox, viewByLocationButton });
    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MainForm());
    }
}
