public class Item {
    public string name;
    public int trait0;
    public int trait1;
    public int trait2;
    public Item() {
        name = ""; trait0 = 0; trait1 = 0; trait2 = 0; /* ... */
    }
    override public string ToString() {
        return "name: " + name + "\ntrait0: " + trait0 + "\ntrait1: " + trait1 + "\ntrait2: " + trait2;
    }
}