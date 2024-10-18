using UnityEngine;
using UnityEngine.UI;  // Import the UI namespace

public class World : MonoBehaviour {

    public static World currentWorld;

    public int chunkWidth = 20, chunkHeight = 20;
    public float viewRange = 30;

    public Chunk chunkFab;
    public InputField seedInputField;  // Reference to the InputField

    public int seed;

    void Awake() {
        Cursor.visible = false;
        currentWorld = this;

        if (seed == 0)
            seed = Random.Range(0, int.MaxValue);

        // Initialize the InputField with the current seed value
        if (seedInputField != null) {
            seedInputField.text = seed.ToString();
            seedInputField.onEndEdit.AddListener(OnSeedChanged); // Listen to seed changes
        }
    }

    void Update() {
        GenerateWorld();
    }

    void GenerateWorld() {
        for (float x = transform.position.x - viewRange; x < transform.position.x + viewRange; x += chunkWidth) {
            for (float z = transform.position.z - viewRange; z < transform.position.z + viewRange; z += chunkWidth) {

                Vector3 pos = new Vector3(x, 0, z);
                pos.x = Mathf.Floor(pos.x / (float)chunkWidth) * chunkWidth;
                pos.z = Mathf.Floor(pos.z / (float)chunkWidth) * chunkWidth;

                Chunk chunk = Chunk.FindChunk(pos);
                if (chunk != null) continue;

                chunk = (Chunk)Instantiate(chunkFab, pos, Quaternion.identity);
            }
        }
    }

    // This method will be called when the seed is changed via the InputField
    public void OnSeedChanged(string newSeedValue) {
        if (int.TryParse(newSeedValue, out int newSeed)) {
            seed = newSeed;
            RegenerateWorld();
        }
    }

    void RegenerateWorld() {
        // Destroy all existing chunks
        Chunk[] chunks = FindObjectsOfType<Chunk>();
        foreach (Chunk chunk in chunks) {
            Destroy(chunk.gameObject);
        }

        // Generate the world again with the new seed
        Random.InitState(seed); // Set the random seed
        GenerateWorld();
    }
}
