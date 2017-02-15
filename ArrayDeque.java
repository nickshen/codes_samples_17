public class ArrayDeque<Item> {
    private int size;

    private int head = 5; //front value pointer
    private int tail = 6; //end value pointer

    private Item[] items;

    //Add and remove must take constant time, except during resizing operations.
    //Get and size must take constant time.
    //The starting size of your array should be 8.

    //The amount of memory that your program uses at any given time must be
    //proportional to the number of items. For example, if you add 10,000 items
    //to the Deque, and then remove 9,999 items, you shouldn't still be using an
    // array of length 10,000ish. For arrays of length 16 or more, your usage
    //factor should always be at least 25%. For smaller arrays, your usage factor
    //can be arbitrarily low.

    public ArrayDeque() {
        items = (Item[]) new Object[8];
    }

    public void addFirst(Item elem) {
        //Adds an item to the front of the Deque.
        if (size == items.length) {
            resize(items.length * 2);
        }

        items[head] = elem;
        head = Math.floorMod(head - 1, items.length);
        size += 1;
    }

    public void addLast(Item elem) {
        //Adds an item to the back of the Deque.
        if (size == items.length) {
            resize(items.length * 2);
        }

        items[tail] = elem;
        tail = Math.floorMod(tail + 1, items.length);
        size += 1;
    }

    private void resize(int capacity) {
        //Resizes array using arraycopy based on size needed
        Item[] enlarged = (Item[]) new Object[capacity];
        //Used for one edge case when tail and head loop
        if (tail == 0 && head == items.length - 1) {
            System.arraycopy(items, 0, enlarged, 0, items.length);
            head = enlarged.length - 1;
            tail = items.length;
        } else { //General circular array resizing
            System.arraycopy(items, 0, enlarged, 0, head + 1);
            System.arraycopy(items, tail, enlarged, tail + capacity / 2, items.length - tail);
            head += capacity / 2;
        }
        items = enlarged;
    }
    //Used to shrink array when usage drops below 25%
    private void shrink(int capacity) {
        Item[] shrunk = (Item[]) new Object[capacity];
        System.arraycopy(items, 0, shrunk, 0, tail);
        System.arraycopy(items, head + 1, shrunk, head + 1 - capacity, items.length - (head + 1));
        items = shrunk;
        head -= capacity;
    }

    public boolean isEmpty() {
        //Returns true if deque is empty, false otherwise.
        return size == 0;
    }

    public int size() {
        //Returns the number of items in the Deque.
        return size;
    }

    public void printDeque() {
        //Prints the items in the Deque from first to last, separated by a space.
        for (int i = head; i < items.length; i++) {
            if (items[i] != null) {
                System.out.print(items[i].toString() + " ");
            }
        }
        for (int i = 0; i < head; i++) {
            if (items[i] != null) {
                System.out.print(items[i].toString() + " ");
            }
        }
        System.out.println();
    }

    public Item removeFirst() {
        //Removes and returns the item at the front of the Deque.
        //If no such item exists, returns null.
        head = Math.floorMod(head + 1, items.length);
        Item removed = items[head];
        items[head] = null;
        size -= 1;

        if (items.length > 16 && (float) size / items.length < 0.25) {
            shrink(items.length / 2);
        }
        return removed;
    }

    public Item removeLast() {
        //Removes and returns the item at the back of the Deque.
        //If no such item exists, returns null.
        tail = Math.floorMod(tail - 1, items.length);
        Item removed = items[tail];
        items[tail] = null;
        size -= 1;

        if (items.length > 16 && (float) size / items.length < 0.25) {
            shrink(items.length / 2);
        }
        return removed;
    }

    //Return value of Item
    public Item get(int index) {
        return items[Math.floorMod(head + (index + 1), items.length)];
    }
}
