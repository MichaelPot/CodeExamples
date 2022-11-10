package build;

import java.io.File;
import java.io.Serializable;
import java.util.LinkedHashMap;
import java.util.Map;

public class Gradebook implements Serializable {

  protected SecretKey key;
  private long timestamp;
  private LinkedHashMap<String, Student> students;
  private LinkedHashMap<String, Assignment> assignments;

  public Gradebook(String filename, String key) {

    File f = new File(filename);
    if (!f.exists() || !filename.matches("[a-zA-Z_.]+"))
      exit("read gradebook filename");

    Gradebook gradebook = Utils.readGradebook(filename);

    SecretKey sk = new SecretKey(key);
    if (!sk.equals(gradebook.key))
      exit("key verification");

    this.key = gradebook.key;
    students = gradebook.getStudents();
    assignments = gradebook.getAssignments();
  }

  public Gradebook(SecretKey key) {
    this.key = key;
    students = new LinkedHashMap<>();
    assignments = new LinkedHashMap<>();
  }

  public static void exit(String m) {
    System.out.println(m + " failed");
    System.exit(255);
  }

  public long getTimestamp() {
    return timestamp;
  }
  public void updateTimestamp() {
    timestamp = System.currentTimeMillis();
  }

  public LinkedHashMap<String, Student> getStudents() {
    return students;
  }

  public LinkedHashMap<String, Assignment> getAssignments() {
      return assignments;
  }

  public void addStudent(String firstname, String lastname) {
    Student student = new Student(firstname, lastname, assignments.values());
    if (students.containsKey(student.getName())) exit("addStudent");
    students.put(student.getName(), student);
    for (Map.Entry<String, Assignment> entry : assignments.entrySet())
      entry.getValue().addGrade(student);
  }

  public void addAssignment(String name, int points, double weight) {
    Assignment assignment = new Assignment(name, points, weight, students.values());
    if (assignments.containsKey(name)) exit("addAssignment");
    assignments.put(name, assignment);
    for (Map.Entry<String, Student> entry : students.entrySet())
      entry.getValue().addGrade(assignment);
  }

  public void deleteAssignment(String name) {
    if (!assignments.containsKey(name)) exit("deleteAssignment");
    Assignment assignment = assignments.remove(name);
    for (Map.Entry<String, Student> entry : students.entrySet())
      entry.getValue().removeGrade(assignment);
  }

  public void deleteStudent(String firstname,String lastname) {
    String name = firstname + " " + lastname;
    if (!students.containsKey(name)) exit("deleteStudent");
    Student student = students.remove(name);
    for (Map.Entry<String, Assignment> entry : assignments.entrySet())
      entry.getValue().removeGrade(student);
  }

  public void addGrade(String firstname, String lastname, String assignmentName, int grade) {
    String name = firstname + " " + lastname;
    if (!students.containsKey(name) || !assignments.containsKey(assignmentName))
      exit("addGrade");
    students.get(name).addGrade(assignments.get(assignmentName), grade);
    assignments.get(assignmentName).addGrade(students.get(name), grade);
  }
}