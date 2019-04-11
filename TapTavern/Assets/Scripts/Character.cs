using UnityEngine;

public class Character : MonoBehaviour
{
    // Base Stats
    private const float MaxHP = 120;
    public static float CurrentHP = MaxHP;


    public float getMaxHP()
    {
        return MaxHP;
    }
    public float gsCurrentHP
    {
        get
        {
            return CurrentHP;
        }
        set
        {
            CurrentHP = value;
        }
    }


    //Priority Calculations
    bool Alive = true;
    public static float ClassPriority = 1;
    static float HBPriority = MaxHP - CurrentHP;

    public bool IsAlive { get => Alive; set => Alive = value; }
    public float CPriority { get => ClassPriority; set => ClassPriority = value; }
    public float HealthPriority { get => HBPriority; set => HBPriority = value; }
    static float AlivePriority;
    public float APriority { get => AlivePriority; set => AlivePriority = value; }
    void Update()
    {
        if (IsAlive)
        {
            AlivePriority = 1;
        }
        else
        {
            AlivePriority = 0;
        }

        if(CurrentHP <= 0)
        {
            IsAlive = false;
        }
    }
    private float TotalPriority = AlivePriority * ClassPriority * HBPriority;
    public float TPriority { get => TotalPriority; set => TotalPriority = value; }

}



