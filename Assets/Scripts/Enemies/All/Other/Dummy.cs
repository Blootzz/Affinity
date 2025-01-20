

public class Dummy : Enemy
{
    public float dummyHealth = 1; // max health
    public float dummyMass = 1;
    public float dummyVulnerability = 0;

    new // Unity prefers this for some reason. This definitively blocks the parent from executing Start(); then I do it manually. Idk


        // Start is called before the first frame update
        void Start()
    {
        health = dummyHealth;
        mass = dummyMass;
        vulnerability = dummyVulnerability;
        flipLocked = true;
        base.Start();
    }

    // Update is called once per frame
    //override void Update()
    //{
    //  base.Update()
    //}

}
