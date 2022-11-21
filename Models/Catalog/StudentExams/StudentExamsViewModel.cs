namespace DaisyStudy.Models.Catalog.StudentExams
{
    public class StudentExamsViewModel
    {
        public int StudentExamID { set; get; }

        public int ExamScheduleID { set; get; }

        public string? StudentID { set; get; }

        public float Mark { set; get; }

        public string? Note { set; get; }

        public DateTime DateTimeStudentExam { set; get; }
    }
}