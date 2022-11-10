package build;

import java.io.File;
import java.security.NoSuchAlgorithmException;

public class setup {

  public static boolean verify_file(String filename) {
    File f = new File(filename);
    return !f.exists() && filename.matches("[a-zA-Z_.]+");
  }

  public static void main(String[] args) throws NoSuchAlgorithmException {

    if (args.length < 2) {
      System.out.println("Usage: setup <logfile pathname>");
      System.exit(1);
    }

    String filename = args[1];
    if (!verify_file(filename))
      Gradebook.exit("setup filename");
    if (!args[0].equals("-N"))
      Gradebook.exit("setup params");

    SecretKey key = new SecretKey();
    Gradebook gradebook = new Gradebook(key);
    Utils.writeGradebook(filename, gradebook);
    System.out.println("Key is: " + key);
  }
}