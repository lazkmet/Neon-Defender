using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyToken : MonoBehaviour
{
    private AudioManager manager;
    private int dif = 0;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        manager = FindObjectOfType<AudioManager>();
    }
    public void Click() {
        manager.Play("Click");
    }
    public void Set(int newDif) {
        dif = newDif;
    }
    public int Get() {
        Invoke(nameof(Clear), 0.1f);
        return dif;
    }
    private void Clear() {
        Destroy(this.gameObject);
    }
}
