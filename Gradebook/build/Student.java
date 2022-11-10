package build;

import java.io.Serializable;
import java.util.Collection;
import java.util.LinkedHashMap;

public class Student implements Serializable {

  private String firstname, lastname;
  private LinkedHashMap<Assignment, Integer> grades;

  public Student(String firstname, String lastname, Collection<Assignment> assignments) {
    this.firstname = firstname;
    this.lastname = lastname;
    grades = new LinkedHashMap<Assignment, Integer>();
    for (Assignment assignment : assignments)
      grades.put(assignment, 0);
  }

  public String getFirstname() {
    return firstname;
  }

  public String getLastname() {
    return lastname;
  }

  public String getName(){
    return firstname + " " + lastname;
  }

  public LinkedHashMap<Assignment, Integer> getGrades() {
    return grades;
  }

  public void addGrade(Assignment assignment) {
      grades.put(assignment, 0);
  }

  public void addGrade(Assignment assignment, int grade) {
      grades.put(assignment, grade);
  }

  public void removeGrade(Assignment assignment) {
    grades.remove(assignment);
  }

  public boolean equals(Object obj) {
    if (this == obj) return true;
    if (obj == null || getClass() != obj.getClass())
      return false;
    Student student = (Student) obj;
    return getName().equals(student.getName());
  }
}