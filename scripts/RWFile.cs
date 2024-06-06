using System;
using System.Diagnostics;
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

    [JsonPropertyName("Stars")]
    public int Stars { get; set; }

    public bool IsDefault()
    {
        return Score == 0 && Wave == 0 && KilledEnemies == 0 && LengthAlive == 0 && Stars == 0;
    }
}
    public class RWFile 
    {
    // Saves data, creates a struct and serializes it to a file
    public void Save(int score, int lengthAlive, int killedEnemies, int wave, int star, int slot){
        try {
        string filePath = $"res://Data/LevelData/Level{slot}.txt";
        var saveData = new SaveStruct {
        Score = score,
        LengthAlive = lengthAlive,
        KilledEnemies = killedEnemies,
        Wave = wave,
        Stars = star
        };

        string data = JsonSerializer.Serialize(saveData);
        using (var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Write)){
            file.StoreString(data);
        }
        }
        catch (Exception ex)
        {
        // Handle the exception here, like logging the error or informing the user
        Debug.Print($"Error saving data: {ex.Message}");
        }
    }

    public SaveStruct Load(int slot) {
        try {
        string filePath;
        
        if (slot == 0) {
        filePath = $"res://Data/PlayerData.txt";
        } else {
        filePath = $"res://Data/LevelData/Level{slot}.txt";
        }

        using (var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read)) {
            return JsonSerializer.Deserialize<SaveStruct>(file.GetAsText());
        }}

        catch (Exception ex){
        // Handle the exception here, like returning a default struct or informing the user
        Debug.Print($"Error loading data: {ex.Message}");
        return new SaveStruct(); // Consider returning a default value here
        }
    }
    }
