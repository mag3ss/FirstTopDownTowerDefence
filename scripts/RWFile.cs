using System;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;
using Godot;
public struct SaveStruct {
    [JsonPropertyName("Score")]
    public int Score { get; set; }

    [JsonPropertyName("LengthAlive")]
    public int LengthAlive { get; set; }

    [JsonPropertyName("KilledEnemies")]
    public int KilledEnemies { get; set; }

    [JsonPropertyName("Wave")]
    public int Wave { get; set; }

    [JsonPropertyName("Health")]
    public int Health { get; set; }

    [JsonPropertyName("Stars")]
    public int Stars { get; set; }
}
    public class RWFile
    {

        // Saves data, creates a struct and serializes it to a file
        public void Save(int score, int lengthAlive, int killedEnemies, int wave, int playerHealth, int star, int slot) {
        try {
            string filePath = $"res://Data/LevelData/Level{slot}.txt";
            var saveData = new SaveStruct {
                Score = score,
                LengthAlive = lengthAlive,
                KilledEnemies = killedEnemies,
                Wave = wave,
                Health = playerHealth,
                Stars = star
            };
            string data = JsonSerializer.Serialize(saveData);
            using (var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Write)) {
                if (file == null) {
                    GD.PrintErr("Failed to open file for writing.");
                    return;
                }
                file.StoreString(data);
            }

            GD.Print("Data saved successfully");
        } catch (Exception ex) {
            GD.PrintErr($"Error saving data: {ex.Message}");
            GD.PrintErr(ex.ToString());
        }   
    }

    public void SaveEndlessScore (int score) {
        string filePath = $"res://Data/PlayerData.txt";
        var saveData = new SaveStruct {
            Score = score
        };
        string data = JsonSerializer.Serialize(saveData);
        using (var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Write)) {
            if (file == null) {
                GD.PrintErr("Failed to open file for writing.");
                return;
            }
            file.StoreString(data);
        }
    }

    // Loads data, deserializes it from a file and returns a struct
    public SaveStruct Load(int slot) {
        string filePath;
        if (slot == 0) {
            filePath = $"res://Data/PlayerData.txt";
        } else {
            filePath = $"res://Data/LevelData/Level{slot}.txt";
        }
        return JsonSerializer.Deserialize<SaveStruct>(FileAccess.Open(filePath, FileAccess.ModeFlags.Read).GetAsText());
        
    }
}
