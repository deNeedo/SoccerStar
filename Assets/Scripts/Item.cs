public class Item {
    public string name;
    public int trait0;
    public int trait1;
    public int trait2;
    public Item() {
        name = ""; trait0 = 0; trait1 = 0; trait2 = 0; /* ... */
    }
    override public string ToString() {
        return "name: " + name + " trait0: " + trait0 + " trait1: " + trait1 + " trait2: " + trait2;
    }
}