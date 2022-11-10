package build;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Collections;
import java.util.Comparator;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

public class gradebookdisplay {

  Gradebook gradebook;

  gradebookdisplay(Gradebook gradebook) {
    this.gradebook = gradebook;
  }

  private class SortA implements Comparator<Student> {
    public int compare(Student s1, Student s2) {
      int last = s1.getLastname().compareTo(s2.getLastname());
      if (last != 0) return last;
      return s1.getFirstname().compareTo(s2.getFirstname());
    }
  }
  private class SortG implements Comparator<Map.Entry<Student, Integer>> {
      public int compare(Map.Entry<Student, Integer> o1, Map.Entry<Student, Integer> o2) {
        return -(o1.getValue().compareTo(o2.getValue()));
    }
  }
  private class SortFG implements Comparator<Map.Entry<Student, Double>> {
      public int compare(Map.Entry<Student, Double> o1, Map.Entry<Student, Double> o2) {
        return -(o1.getValue().compareTo(o2.getValue()));
    }
  }

  private void print_Assignment(String assignmentName, boolean alphabetical) {
    Assignment assignment = gradebook.getAssignments().get(assignmentName);
    LinkedHashMap<Student, Integer> grades = assignment.getGrades();
    if (alphabetical) {
      ArrayList<Student> keys = new ArrayList<>(grades.keySet());
      Collections.sort(keys, new SortA());
      for (Student s : keys)
        System.out.println(String.format("(%s, %s, %d)", s.getLastname(), s.getFirstname(), grades.get(s)));
    } else {
      List<Map.Entry<Student, Integer>> list = new ArrayList<>(grades.entrySet());
      Collections.sort(list, new SortG());
      for (Map.Entry<Student, Integer> entry : list) {
        Student s = entry.getKey();
        System.out.println(String.format("(%s, %s, %d)", s.getLastname(), s.getFirstname(), entry.getValue()));
      }
    }
  }

  private void print_Student(String firstname, String lastname) {
    String name = firstname + " " + lastname;
    Student student = gradebook.getStudents().get(name);
    LinkedHashMap<Assignment, Integer> grades = student.getGrades();
    for (Map.Entry<Assignment, Integer> entry : grades.entrySet())
      System.out.println(String.format("(%s, %d)", entry.getKey().getName(), entry.getValue()));
  }

  private void print_Final(boolean alphabetical) {
    Collection<Student> students = gradebook.getStudents().values();
    LinkedHashMap<Student, Double> finalGrades = new LinkedHashMap<>();
    for (Student student : students) {
      double grade = 0;
      for (Map.Entry<Assignment, Integer> entry : student.getGrades().entrySet()) {
        Assignment a = entry.getKey();
        grade += a.getWeight() * entry.getValue() / a.getPoints();
      }
      finalGrades.put(student, grade);
    }
    if (alphabetical) {
      ArrayList<Student> keys = new ArrayList<>(finalGrades.keySet());
      Collections.sort(keys, new SortA());
      for (Student s : keys)
        System.out.println("(" + s.getLastname() + ", " + s.getFirstname() + ", " + finalGrades.get(s) + ")");
    } else {
      List<Map.Entry<Student, Double>> list = new ArrayList<>(finalGrades.entrySet());
      Collections.sort(list, new SortFG());
      for (Map.Entry<Student, Double> entry : list) {
        Student s = entry.getKey();
        System.out.println("(" + s.getLastname() + ", " + s.getFirstname() + ", " + entry.getValue() + ")");
      }
    }
  }

  private static void validateOrder(ArrayList<String> params) {
    if (!params.get(0).equals("-N") || 
        !params.get(1).equals("-K") ||
        !params.get(2).matches("-PA|-PS|-PF"))
      Gradebook.exit("validate order");

    for (String param : params.subList(3, params.size()))
      if (!param.matches("-AN|-FN|-LN|-A|-G"))
        Gradebook.exit("validate order");
  }

  public static void main(String[] args) {
    LinkedHashMap<String, String> settings = new LinkedHashMap<>();
    if(args.length==1)
      System.out.println("\nNo Extra Command Line Argument Passed Other Than Program Name");
    if(args.length>=2) {
      System.out.println("\nNumber Of Arguments Passed: %d" + args.length);
      System.out.println("----Following Are The Command Line Arguments Passed----");
      for(int counter = 0; counter < args.length; counter++) {
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
    validateOrder(new ArrayList<String>(settings.keySet()));

    String filename = settings.remove("-N");
    String key = settings.remove("-K");

    Gradebook gradebook = new Gradebook(filename, key);
    gradebookdisplay display = new gradebookdisplay(gradebook);

    if (settings.containsKey("-PA")) {
      settings.remove("-PA");

      if (!settings.containsKey("-AN"))
        Gradebook.exit("PA params");
      
      if (!settings.containsKey("-A") && !settings.containsKey("-G"))
        Gradebook.exit("PA params");

      boolean alphabetical = settings.containsKey("-A");
      settings.remove(alphabetical ? "-A" : "-G");
      String assignmentName = settings.remove("-AN");

      if (!settings.isEmpty()) Gradebook.exit("params");
      display.print_Assignment(assignmentName, alphabetical);
    } else if (settings.containsKey("-PS")) {
      settings.remove("-PS");

      for (String option : new String[]{"-FN", "-LN"}) {
        if (!settings.containsKey(option))
          Gradebook.exit("PS params");
      }

      String firstname = settings.remove("-FN");
      String lastname = settings.remove("-LN");

      if (!settings.isEmpty()) Gradebook.exit("params");
      display.print_Student(firstname, lastname);
    } else if (settings.containsKey("-PF")) {
      settings.remove("-PF");

      if (!settings.containsKey("-A") && !settings.containsKey("-G"))
        Gradebook.exit("PF params");

      boolean alphabetical = settings.containsKey("-A");
      settings.remove(alphabetical ? "-A" : "-G");

      if (!settings.isEmpty()) Gradebook.exit("params");
      display.print_Final(alphabetical);
    }
  }
}