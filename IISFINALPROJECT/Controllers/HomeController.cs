using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using System.IO.Ports;
using System.IO;
using System.Drawing;
using System.Data;
using System.Threading.Tasks;
using IISFINALPROJECT.Models;
using System.Drawing.Imaging;


namespace IISFINALPROJECT.Controllers
{
    public class HomeController : Controller
    {
     
        public ActionResult Index()
        {
           // Student stud = getStudent();
            return View();
        }
       
//========================================================EditProfile=============================================================
        [HttpGet]
        public ActionResult EditProfile()
        {  
            Student data = (Student)Session["student"];
            return View(data);
        }
        [HttpPost]
        public ActionResult editProfile(Student student)
        {

            Student students = student;

           // if (ModelState.IsValid)
           // {
           try { 
                 using (var db = new ProjectDatabaseEntities1())
                {
                    Student stud=  db.Student.SingleOrDefault(s=>s.Sudent_Number==students.Sudent_Number);
                    //Student stud = db.Student.Find(students.Sudent_Number);
                    stud.Name = "Brightwell";
                    //stud.Surname = student.Surname;
                     // stud.Password = student.Password;
                        db.Entry(stud).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();         
                }
            }
           catch
           {
           }
           if (student.Who == "Student")
           {
               return View("DashboardStudent", getList());
           }
           else
           {
               return RedirectToAction("/DashboardAdmin1");
           }
            //return View("DashboardStudent", getList());
        }
//========================================================Registration============================================================
        [HttpGet]
        public ActionResult Registration()
        {
            ProjectDatabaseEntities1 data = new ProjectDatabaseEntities1();
            var c = data.Course.ToList();
            SelectList d = new SelectList(c, "Code", "Name");
            ViewBag.courseList = d;
            return View();
        }
        [HttpPost]
        public ActionResult Registration(Student student,Image FingerPic)
        {
            if (check(student)==false)
            {
                try
                {
                    using (var db = new ProjectDatabaseEntities1())
                    {
                        student.Fingerprints ="ddd";
                        db.Student.Add(student);
                        db.SaveChanges();
                    }
                    Session["student"] = student;
                    if (student.Who == "Student")
                    {
                        return View("DashboardStudent", getList());
                    }
                    else
                    {
                        return RedirectToAction("/DashboardAdmin1");
                    }
                    return View("userProfile", student);
                }
                catch
                {
                    
                }   
            }
            return View();
        }
        public bool check(Student student)
        {
            bool exist = false;
            using (var db = new ProjectDatabaseEntities1())
            {
                //var data = db.Students.Where(x => x.Sudent_Number == student.Sudent_Number).FirstOrDefault(); ;
                var data = db.Student.Find(student.Sudent_Number);
                if (data != null)
                {
                    exist = true;
                    ViewBag.message = "Student exist";
                }
            }
            return exist;
        }
// =========================================================Login=================================================================
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Student student)
        {
            bool step = false;
            using (var db = new ProjectDatabaseEntities1())
            {
                var t = db.Student.Where(x => x.Sudent_Number == student.Sudent_Number && x.Password == student.Password).FirstOrDefault(); 
                student = t;
                if (t != null)
                {
                    step = true;
                }
            }
            
            if(step){
                Session["student"] = student;
                if (student.Who == "Student")
                {
                    return View("DashboardStudent", getList());
                }
                else
                {
                    return RedirectToAction("/DashboardAdmin1");
                }
            }
            else
            {
                ViewBag.status = false;
                ViewBag.message = "Student doesnt exist";
                return View();
            }
        }
//======================================================Student Dashboard============================================================================
        public ActionResult DashboardStudent()
        {
             A data = getList();
             return View(data);
        }
        public A getList(){
            try{
           A data = new  A(); 
           List<Course> courseL = new List<Course>();
           List<Module> moduleL = new List<Module>();
           List<StudentModule> moduleS = new List<StudentModule>();
           List<StudentAssignment> assignList = new List<StudentAssignment>();
            using (var db = new ProjectDatabaseEntities1())
            {
                Student student = (Student)Session["student"] ;
                courseL = db.Course.Where(c=>c.Code==student.Course).ToList();
                moduleS = db.StudentModule.Where(m => m.StudentNum == student.Sudent_Number).ToList();
                assignList = db.StudentAssignment.Where(m => m.StudentNumber == student.Sudent_Number).ToList();
                moduleL = db.Module.ToList();
            }
            data.ListCourse = courseL;
            data.ListModule = moduleL;
            data.ListStudentModules = moduleS;
            data.ListStudentAssignment = assignList;
            data.ListAssignment = GetA();
            return data;
            }
            catch{

            }
            return new A();
        }
        public List<Assigments> GetA()
        {
            List<Assigments> assignList = new List<Assigments>();
            using (var db = new ProjectDatabaseEntities1())
            {
                assignList = db.Assigments.ToList();
            }
            return assignList;
        }
//===================================================Dashboard====================================================================

        public ActionResult DashboardAdmin1()
        {
            Student student = (Student)Session["student"];
            ViewBag.student = student;
            List<Module> checkAdmin = new List<Module>();
            using (var db = new ProjectDatabaseEntities1())
            {
                checkAdmin = db.Module.Where(a => a.Lecture == student.Sudent_Number).ToList();
            }
            A line = new A();
            line.ListModule = checkAdmin;
            return View(line);
        }

        [HttpGet]
        public ActionResult DashboardAdmin()
        {
            string module = (string)Session["Module"];
            Student student = (Student)Session["student"];
            ViewBag.student = student;
            A line = getData(student, module);
            return View(line);
        }
        [HttpPost]
        public ActionResult DashboardAdmin(string module)
        {
            Session["Module"] = module;
            Student student = (Student)Session["student"];
            ViewBag.student = student;
            A line = getData(student,module);
            return View(line);
        }
        public A getData(Student s,string moduleKey)
        {
            try
            {
                List<StudentModule> stuModules = new List<StudentModule>();
                List<Student> students = new List<Student>();
                List<Module> checkAdmin = new List<Module>();
                List<StudentAssignment> studentAss = new List<StudentAssignment>();
                List<Assigments> assigList = new List<Assigments>();
                using (var db = new ProjectDatabaseEntities1())
                {
                   Module module = db.Module.Where(a => a.Key== moduleKey).FirstOrDefault();
                    checkAdmin.Add(module);
                    //checkAdmin = db.Module.Where(a => a.Lecture == s.Sudent_Number).ToList();
                }
                using (var db = new ProjectDatabaseEntities1())
                {
                    assigList = db.Assigments.Where(a=>a.Module==moduleKey).ToList();
                    foreach (var c in checkAdmin)
                    {
                        stuModules = db.StudentModule.Where(a => a.ModuleCode == c.Key).ToList();
                    }
                    foreach (var d in stuModules)
                    {
                        //Student student = db.Student.Where(m => m.Sudent_Number == d.StudentNum).FirstOrDefault();
                        //students.Add(student);
                        students = db.Student.Where(m => m.Sudent_Number == d.StudentNum).ToList();
                    }
                    //foreach (var asn in assigList)
                    //{
                    //    studentAss = db.StudentAssignment.ToList();
                    //    studentAss = db.StudentAssignment.Where(a => a.Assignment == asn.Id).ToList();
                    //}
                    studentAss = db.StudentAssignment.ToList();
                   // studentAss = db.StudentAssignment.Where(a=>a.Assignment).ToList();
                }
                A line = new A();
                line.ListStudent = students;
                line.ListModule = checkAdmin;
                line.ListStudentAssignment = studentAss;
                line.ListAssignment = assigList;
                return line;
            }
            catch
            {
            }
            return new A();
        } 
//===============================================Add Module===================================================================================
        [HttpGet]
        public ActionResult addModule()
        {
            try
            {
                ProjectDatabaseEntities1 data = new ProjectDatabaseEntities1();
                var c = data.Course.ToList();
                SelectList d = new SelectList(c, "Code", "Name");
                ViewBag.courseList = d;
            }
            catch
            {
            }
            return View();
        }

        [HttpPost]
        public ActionResult addModule(Module module)
        {   if (ModelState.IsValid)
                {
                    List<Assigments> b = new List<Assigments>();
                    List<Module> m = new List<Module>();
                    Student student = (Student)Session["student"];
                    module.Lecture = student.Sudent_Number.ToString();                  
                    using (var db = new ProjectDatabaseEntities1())
                    {
                        db.Module.Add(module);
                        Assigments assign = new Assigments();
                        assign.Assigment = "Final M";
                        assign.Mark = "100";
                        b = db.Assigments.ToList();
                        assign.Id = (b.Count + 1);
                        assign.Module = module.Key;
                        db.Assigments.Add(assign);
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("/DashboardAdmin");
           // return RedirectToAction("/DashboardAdmin");
        }
        public ActionResult addAssignments(Module module)
        {
            return View();
        }
//====================================================Register a module===============================================================
        public ActionResult registerModule()
        {
            try { 
                  List<StudentModule> assignList = new List<StudentModule>();
                List<Module> moduleList = new List<Module>();
                ProjectDatabaseEntities1 data = new ProjectDatabaseEntities1();
                var c = data.Module.ToList();
                SelectList d = new SelectList(c, "Key", "Key");
                ViewBag.moduleList = d;
            }
            catch
            {

            }
            return View();
        }
        [HttpPost]
        public ActionResult registerModule(Module module)
        {
            try
            {
                Student student = (Student)Session["student"];
                StudentModule smodule = new StudentModule();
                List<Assigments> assign = new List<Assigments>();
                smodule.StudentNum = student.Sudent_Number;
                smodule.ModuleCode = module.Key;
                smodule.Id = 6;
                using (var db = new ProjectDatabaseEntities1())
                {
                    List<StudentModule> m=db.StudentModule.ToList();
                    smodule.Id = (int)(m.Count+1);
                    db.StudentModule.Add(smodule);
                    db.SaveChanges();
                    assign = db.Assigments.Where(xx => xx.Module == smodule.ModuleCode).ToList();
                    foreach (var assm in assign)
                    {
                        StudentAssignment sta = new StudentAssignment();
                        List<StudentAssignment> ac = db.StudentAssignment.ToList();
                        sta.Assignment = assm.Id;
                        sta.Mark = "0";
                        sta.StudentNumber = student.Sudent_Number;
                        sta.Id = (int)(ac.Count) + 1;
                        db.StudentAssignment.Add(sta);
                        db.SaveChanges();
                    }
                }
                return View("DashboardStudent", getList());
            }
            catch
            {
            }
            return View("DashboardStudent", getList());
        }
//=======================================================update marks=============================================================================        
      
        [HttpPost]
        public ActionResult updateMarks(string assign1, string identity1, string Assign2, string identity2, string Studentno)
        {
           // try
            //{
                string mark = assign1;
                string[] Assg1 = identity1.Split(',');
                string[] Assg2 = identity2.Split(',');

                StudentAssignment sta = new StudentAssignment();
                sta.Id = Convert.ToInt32(Assg1[0]);
                sta.Mark = mark;
                sta.StudentNumber = Studentno;
                sta.Assignment = Convert.ToInt32(Assg1[1]);

                StudentAssignment sta2 = new StudentAssignment();
                sta2.Id = Convert.ToInt32(Assg2[0]);
                sta2.Mark = Assign2;
                sta2.StudentNumber = Studentno;
                sta2.Assignment = Convert.ToInt32(Assg2[1]);
                using (var db = new ProjectDatabaseEntities1())
                {
                    StudentAssignment sa = db.StudentAssignment.SingleOrDefault(c => c.Id == sta.Id && c.StudentNumber == Studentno);
                    sa.Mark = sta.Mark;
                    StudentAssignment sa2 = db.StudentAssignment.SingleOrDefault(c => c.Id == sta2.Id && c.StudentNumber == Studentno);
                    sa2.Mark = sta2.Mark;
                    db.SaveChanges();
                }
            //}
            //catch
            //{
            //}
            return RedirectToAction("/DashboardAdmin");
        }


        SerialPort port = new SerialPort("COM5", 9600); 
        [HttpGet]
        public ActionResult SerialConnect()
        {
            //port.Open();
            return View();
        }
        [HttpPost]
        public ActionResult SerialConnect(string state )
        {
           port.Open();
                port.Write(state + '\n');

          
            port.Close();
            return View();  
        }
        [HttpGet]
        public ActionResult checkImage()
        {
            return View();  
        }

        //[HttpPost]
        //public ActionResult checkImage(Image image1, File image2)
        //{
        //    Bitmap aa = new Bitmap(image1);
        //    Bitmap bb = new Bitmap(image2.);
        //    bool a = compare(aa, bb);
        //    if (a)
        //        ViewBag.message = "Same";
        //    else
        //        ViewBag.message = "Not the same";           
        //    return View();  
        //}






        public bool compare(Bitmap image1,Bitmap image2)
        {
            MemoryStream ms = new MemoryStream();
            image1.Save(ms, ImageFormat.Png);
            string first = Convert.ToBase64String(ms.ToArray());
            ms.Position = 0;

            image2.Save(ms, ImageFormat.Png);
            string second = Convert.ToBase64String(ms.ToArray());
            //ms.Position = 0;
            if (first.Equals(second))
            {
                return true;
            }
            return false;
        }

//====================================================================================================================================          
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("/Index");      
        }      
//================================================================================================================================ 
    }
}