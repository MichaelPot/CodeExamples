package build;

import java.io.Serializable;
import java.util.Collection;
import java.util.LinkedHashMap;

public class Assignment implements Serializable {

    private String name;
    private int points;
    private double weight;
    private LinkedHashMap<Student, Integer> grades;

    public Assignment(String name, int points, double weight, Collection<Student> students) {
        this.name = name;
        this.points = points;
        this.weight = weight;
        grades = new LinkedHashMap<Student, Integer>();
        for (Student student : students)
            grades.put(student, 0);
    }

    public String getName() {
        return name;
    }

    public int getPoints() {
        return points;
    }

    public double getWeight() {
        return weight;
    }

    public LinkedHashMap<Student, Integer> getGrades() {
        return grades;
    }

    public void addGrade(Student student) {
        grades.put(student, 0);
    }

    public void addGrade(Student student, int grade) {
        grades.put(student, grade);
    }

    public void removeGrade(Student student) {
        grades.remove(student);
    }

    public boolean equals(Object obj) {
        if (this == obj) return true;
        if (obj == null || getClass() != obj.getClass())
        return false;
        Assignment assignment = (Assignment) obj;
        return getName().equals(assignment.getName());
    }
}