using UnityEngine;

public class VampAIHelper : ScriptableObject
{
    Board boardScript = Board.boardScript;
    public void UpdateVampSpawningEffects() {
        for (int x = 0;x < 7;x++)
            for (int y = 0;y < 5;y++) 
                boardScript.spawnedPieces[x,y].GetComponent<Animator>().SetBool("SpawningSkull",false);
        for(int i = 0; i < boardScript.quedVampSpawnPositions.Length;i++) {
            boardScript.spawnedPieces[boardScript.quedVampSpawnPositions[i].x,boardScript.quedVampSpawnPositions[i].y].GetComponent<Animator>().SetBool("SpawningSkull",true);
        }
    }
}
