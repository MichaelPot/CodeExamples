package build;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;

public class Utils {
    public static void writeGradebook(String filename, Gradebook gradebook) {
        try {
            FileOutputStream file = new FileOutputStream(filename);
            ObjectOutputStream objStream = new ObjectOutputStream(file);
            gradebook.updateTimestamp();
            objStream.writeObject(gradebook);
            objStream.close();
            file.close();
        } catch (Exception e) {
            Gradebook.exit("write gradebook");
        }
    }

    public static Gradebook readGradebook(String filename) {
        Gradebook gradebook = null;
        try {
            long lm = new File(filename).lastModified();
            FileInputStream file = new FileInputStream(filename);
            ObjectInputStream objStream = new ObjectInputStream(file);
            gradebook = (Gradebook) objStream.readObject();
            long ts = gradebook.getTimestamp();
            System.out.println(lm - ts);
            System.out.println("lastModified: " + lm);
            System.out.println("timestamp: " + ts);
            objStream.close();
            file.close();
            if (gradebook.getTimestamp() != lm)
                Gradebook.exit("tampering detected read gradebook");
        } catch(Exception e) {
            Gradebook.exit("read gradebook");
        }
        return gradebook;
    }
}