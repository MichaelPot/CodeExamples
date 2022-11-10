package build;

import java.util.ArrayList;
import java.util.LinkedHashMap;

public class gradebookadd {

  private static LinkedHashMap<String, String> parse_cmdline(String[] args) {
    LinkedHashMap<String, String> settings = new LinkedHashMap<>();
    if(args.length==0) {
      System.out.println("\nNo Extra Command Line Argument Passed Other Than Program Name");
      Gradebook.exit("No extra command line arguments");
    } else if(args.length>=1) {
      System.out.println("\nNumber Of Arguments Passed: " + args.length);
      System.out.println("----Following Are The Command Line Arguments Passed----");
      for(int counter=0; counter < args.length; counter++) {
        System.out.println("args[" + counter + "]: " + args[counter]);
        String arg = args[counter];
        if (arg.charAt(0) == '-')
          settings.put(arg, null);
        else if (settings.containsKey(args[counter - 1]))
          settings.put(args[counter - 1], arg);
        else
          Gradebook.exit("settings");
      }
    }
    return settings;
  }

  private static void validateOrder(ArrayList<String> params) {
    if (!params.get(0).equals("-N") || 
        !params.get(1).equals("-K") ||
        !params.get(2).matches("-AA|-DA|-AS|-DS|-AG"))
      Gradebook.exit("validate order");

    for (String param : params.subList(3, params.size()))
      if (!param.matches("-AN|-FN|-LN|-P|-W|-G"))
        Gradebook.exit("validate order");
  }

  private static boolean invalidAN(String name) {
      return !name.matches("[a-zA-Z0-9]+");
  }

  private static boolean invalidName(String name) {
      return !name.matches("[a-zA-Z]+");
  }

  public static void main(String[] args) {
    LinkedHashMap<String, String> settings = parse_cmdline(args);
    validateOrder(new ArrayList<String>(settings.keySet()));

    String filename = settings.remove("-N");
    String key = settings.remove("-K");

    Gradebook gradebook = new Gradebook(filename, key);

    if (settings.containsKey("-AA")) {
      settings.remove("-AA");

      for (String option : new String[]{"-AN", "-P", "-W"}) {
        if (!settings.containsKey(option))
          Gradebook.exit("AA params");
      }

      String name = settings.remove("-AN");
      int points = Integer.parseInt(settings.remove("-P"));
      double weight = Double.parseDouble(settings.remove("-W"));

      if (invalidAN(name) || points < 0 || weight < 0 || weight > 1)
        Gradebook.exit("AA options");
      
      if (!settings.isEmpty()) Gradebook.exit("params");
      gradebook.addAssignment(name, points, weight);
    } else if (settings.containsKey("-DA")) {
      settings.remove("-DA");

      if (!settings.containsKey("-AN"))
        Gradebook.exit("DA params");

      String name = settings.remove("-AN");

      if (invalidAN(name))
        Gradebook.exit("DA options");
      
      if (!settings.isEmpty()) Gradebook.exit("params");
      gradebook.deleteAssignment(name);
    } else if (settings.containsKey("-AS")) {
      settings.remove("-AS");

      for (String option : new String[]{"-FN", "-LN"}) {
        if (!settings.containsKey(option))
          Gradebook.exit("AS params");
      }

      String firstname = settings.remove("-FN");
      String lastname = settings.remove("-LN");

      if (invalidName(firstname) || invalidName(lastname))
        Gradebook.exit("AS options");

      if (!settings.isEmpty()) Gradebook.exit("params");
      gradebook.addStudent(firstname, lastname);
    } else if (settings.containsKey("-DS")) {
      settings.remove("-DS");

      for (String option : new String[]{"-FN", "-LN"}) {
        if (!settings.containsKey(option))
          Gradebook.exit("DS params");
      }

      String firstname = settings.remove("-FN");
      String lastname = settings.remove("-LN");

      if (invalidName(firstname) || invalidName(lastname))
        Gradebook.exit("DS options");

      if (!settings.isEmpty()) Gradebook.exit("params");
      gradebook.deleteStudent(firstname, lastname);
    } else if (settings.containsKey("-AG")) {
      settings.remove("-AG");

      for (String option : new String[]{"-FN", "-LN", "-AN", "-G"}) {
        if (!settings.containsKey(option))
          Gradebook.exit("AG params");
      }

      String firstname = settings.remove("-FN");
      String lastname = settings.remove("-LN");
      String assignmentName = settings.remove("-AN");
      int grade = Integer.parseInt(settings.remove("-G"));

      if (invalidName(firstname) || invalidName(lastname) 
          || invalidAN(assignmentName) || grade < 0)
        Gradebook.exit("AG options");

      if (!settings.isEmpty()) Gradebook.exit("params");
      gradebook.addGrade(firstname, lastname, assignmentName, grade);
    }

    Utils.writeGradebook(filename, gradebook);
  }
}