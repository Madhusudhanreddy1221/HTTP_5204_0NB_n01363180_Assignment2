using System;
using System.Collections.Generic;
using System.Data;
//required for SqlParameter class
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PetGrooming.Data;
using PetGrooming.Models;
using System.Diagnostics;

namespace PetGrooming.Controllers
{
    public class SpeciesController : Controller
    {
        private PetGroomingContext db = new PetGroomingContext();
        // GET: Species
        public ActionResult Index()
        {
            return View();
        }

        //TODO: Each line should be a separate method in this class
        // List
        public ActionResult List()
        {
            //what data do we need?
            List<Species> myspecies = db.Species.SqlQuery("Select * from species").ToList();

            return View(myspecies);
        }



        // Show
        // GET: Pet/Details/5
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Pet pet = db.Pets.Find(id); //EF 6 technique
            Species species = db.Species.SqlQuery("select * from species where SpeciesID=@SpeciesID", new SqlParameter("@SpeciesID", id)).FirstOrDefault();
            if (species == null)
            {
                return HttpNotFound();
            }
            return View(species);
        }
        // Add
        public ActionResult Add()
        {
            return View();
        }
        // [HttpPost] Add
        [HttpPost]
        public ActionResult Add(String SpeciesName)
        {   //query to add new species into the species table
            string query = "insert into species (name) values (@SpeciesName)";
            SqlParameter parameter = new SqlParameter("@SpeciesName",SpeciesName);
            db.Database.ExecuteSqlCommand(query,parameter);
            return RedirectToAction("List");
        }
        // Update 
        public ActionResult Update(int id)
        {   //query to slect  the particular species from table
            string query = "Select * from species where  SpeciesID=@id";
            SqlParameter parameter = new SqlParameter("@id", id);
            Species selectedspecies = db.Species.SqlQuery(query, parameter).FirstOrDefault();
            return View(selectedspecies);
        }
        // [HttpPost] Update
        [HttpPost]
        public ActionResult Update(int id, String SpeciesName)
        {   //query to update the particualar species based on the id
            string query = "update species set name=@SpeciesName where SpeciesID=@id";
            //key pair values to store species details
            SqlParameter[] parameter = new SqlParameter[2];
            parameter[0] = new SqlParameter("@SpeciesName", SpeciesName);
            parameter[1] = new SqlParameter("@id",id);
            //excecuting the query to update
            db.Database.ExecuteSqlCommand(query, parameter);
            //retunring to list view of the species after adding
            return RedirectToAction("List");
        }
        // (optional) delete
        public ActionResult Delete(int id)
        {   //Query to delete particualr species from the table based on the species id
            string query = "delete from species where SpeciesID=@id";
            SqlParameter[] parameter = new SqlParameter[1];
            //storing the id of the species to be deleted 
            parameter[0] = new SqlParameter("@id", id);
            //Excecuting the query
            db.Database.ExecuteSqlCommand(query, parameter);
           // returning to lsit view of the species after deleting 
            return RedirectToAction("List");
        }

    }
}