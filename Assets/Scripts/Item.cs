public class Item {
    public string name;
    public int trait0;
    public int trait1;
    public int trait2;
    public Item(string name, int trait0, int trait1, int trait2) {
        this.name = name;
        this.trait0 = trait0;
        this.trait1 = trait1;
        this.trait2 = trait2;
    }
    public static void Create() {
        NetworkManager.GenerateItem(PlayerManager.Get());
    }

    public string ToString() {
        if (trait0 != 0) {
            return "name: " + this.name + " trait0: " + this.trait0;
        } else if (trait1 != 0) {
            return "name: " + this.name + " trait1: " + this.trait1;
        } else {
            return "name: " + this.name + " trait2: " + this.trait2;
        }
    }
}