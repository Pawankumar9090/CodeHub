using CodeHub.Data;
using CodeHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CodeHub.Controllers
{
    public class QuestionsManageController : Controller
    {
        private readonly MyDbContext _dbContext;
        private readonly UserManager<UserAccounts> _userManager;
        private readonly SignInManager<UserAccounts> _SignInManager;

        public QuestionsManageController(MyDbContext dbContext, UserManager<UserAccounts> userManager, SignInManager<UserAccounts> signInManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _SignInManager = signInManager;

        }
        public IActionResult Question()
        {
            //if add pagination then .OrderBy(x => x.UserName).Skip(3).Take(5)
            ViewData["AllQ"] = _dbContext.Questions.ToList();
            ViewData["Qlist"] = _dbContext.Questions.Where(x=>x.UserId== _userManager.GetUserId(HttpContext.User)).ToList();
            ViewData["totalQ"] = _dbContext.Questions.Count().ToString();
            TempData["SelectNaveIteam"] = "Question";
            return View();
        }
        [Authorize(Roles ="Admin,User")]
        public IActionResult RaiseQuestion(Questions questions)
        {
            if(questions != null && questions.Question!=null && questions.Question!="") 
            {
                questions.UserId = _userManager.GetUserId(HttpContext.User);
                questions.Date = DateTime.Now;
                questions.UserName=_userManager.FindByIdAsync(questions.UserId).Result.Name;
                questions.AnsOrNot = "No";
                _dbContext.Questions.Add(questions);
                _dbContext.SaveChanges();
                TempData["success"] = "Question Raised";
                TempData["SelectNaveIteam"] = "Question";
                return RedirectToAction("Question", "QuestionsManage");
            }
            TempData["SelectNaveIteam"] = "RaiseQuestion";
            return View();
        }

        [HttpGet]                         
        public IActionResult GetSelectedLangQ(Questions questions)
        {            
            if(questions!=null && questions.QId!=0 && questions.QuestionTage!=null && questions.QuestionTage != "" && questions.UserId!=null && questions.UserId != "")
            {                
                ViewData["selectedQ"] =_dbContext.Questions.Find(questions.QId);
                ViewData["selectedQ_A"] = _dbContext.Answers.Where(x=>x.QId==questions.QId).ToList();
                if (_SignInManager.IsSignedIn(User))
                {
                    ViewData["selectedLangQ"]=_dbContext.Questions.Where(x=>x.UserId==questions.UserId && x.QuestionTage==questions.QuestionTage).ToList();                   
                }
                else
                {
                    ViewData["selectedLangQ"] = _dbContext.Questions.Where(x => x.QuestionTage == questions.QuestionTage).ToList();
                }
                TempData["SelectNaveIteam"]=questions.QuestionTage;
                return View();                              
            }
            else if(questions!=null && questions.QuestionTage!="" && questions.QuestionTage != null)
            {
                List<Questions> qlist= _dbContext.Questions.Where(x => x.QuestionTage == questions.QuestionTage).ToList();
                ViewData["selectedLangQ"] = qlist;
                TempData["SelectNaveIteam"] = questions.QuestionTage;
                if(qlist!=null && qlist.Count > 0)
                {
                    ViewData["selectedQ"] = qlist[0];
                }
                return View();
            }
            //TempData["error"] = "Somthing went wrong try again!";
             return View();
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public IActionResult GetSelectedLangQ(Answer answer)
        {
            if (answer != null)
            {
                
                _dbContext.Answers.Add(answer);
                _dbContext.SaveChanges();
                //Questions questions = new()
                //{
                //    QId= answer.QId,
                //    AnsOrNot="Yes",
                //};
                TempData["success"]= "Answer Submied!";
                return RedirectToAction("GetSelectedLangQ", "QuestionsManage", new { answer.QId, QuestionTage = answer.AnsTage, answer.UserId });
            }
            //TempData["error"] = "Somthing went Wrong please try again!";
            return RedirectToAction("GetSelectedLangQ", "QuestionsManage");
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public  int VotingMethod(int ansid,int vote)
        {
            
            var userid=_userManager.GetUserId(HttpContext.User);
            var gg=_dbContext.Voting.Where(x => x.UserId == userid && x.AnsId == ansid).ToList().Count;
            if (userid != null)
            {
                if(gg>0)
                {
                    Voting voteobj= _dbContext.Voting.Where(x=>x.UserId==userid && x.AnsId == ansid).ToList()[0];
                    if (voteobj.Vote == 0)
                    {
                        voteobj.Vote = vote;
                        _dbContext.Voting.Update(voteobj);
                        _dbContext.SaveChanges();
                        Answer answer= (Answer)_dbContext.Answers.Where(x=>x.AnsId==ansid);
                        if (answer != null)
                        {
                            answer.Vote += vote;
                            _dbContext.Answers.Update(answer);
                            _dbContext.SaveChanges();
                            return answer.Vote;
                        }
                    }
                    else if (vote == -1 && voteobj.Vote==1)
                    {
                        voteobj.Vote = -1;
                        _dbContext.Voting.Update(voteobj);
                        _dbContext.SaveChanges();
                        Answer answer = _dbContext.Answers.Find(ansid);
                        if (answer != null)
                        {
                            answer.Vote += vote;
                            _dbContext.Answers.Update(answer);
                            _dbContext.SaveChanges();
                            return answer.Vote;
                        }
                    }
                    else if (vote == 1 && voteobj.Vote == -1)
                    {
                        voteobj.Vote = 1;
                        _dbContext.Voting.Update(voteobj);
                        _dbContext.SaveChanges();
                        Answer answer = _dbContext.Answers.Find(ansid);
                        if (answer != null)
                        {
                            answer.Vote += vote;
                            _dbContext.Answers.Update(answer);
                            _dbContext.SaveChanges();
                            return answer.Vote;
                        }
                    }
                    else if (vote == voteobj.Vote )
                    {
                        Answer answer = _dbContext.Answers.Find(ansid);
                        if (answer != null)
                        {                       
                            return answer.Vote;
                        }
                    }
                    return 0;
                }
                else
                {
                    if (ansid > 0)
                    {
                        Voting tempvote = new() { UserId = userid, AnsId = ansid, Vote = vote };
                        _dbContext.Voting.Add(tempvote);
                        _dbContext.SaveChanges();
                        Answer answer = _dbContext.Answers.Find(ansid);
                        if (answer != null)
                        {
                            answer.Vote += tempvote.Vote;
                            _dbContext.Answers.Update(answer);
                            _dbContext.SaveChanges();
                            return answer.Vote;
                        }
                    }
                }
            }            
            return 0;
        }
    }
}


