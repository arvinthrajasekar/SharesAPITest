using Microsoft.AspNetCore.Mvc;
using Moq;
using SharesAPI.Controllers;
using SharesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SharesAPITest
{
    public class UnitTest1
    {
        private readonly Mock<ISharesRepository> _mockRepo;
       

        public UnitTest1()
        {
            _mockRepo = new Mock<ISharesRepository>();
           
        }
        [Fact]
        public void GetALLTest()
        {
            var MockShares = SampleRepo();

            _mockRepo.Setup(repo => repo.AllShares())
                   .Returns(MockShares);

            var _controller = new HomeController(_mockRepo.Object);

            var actionresult = _controller.GetALL();
            var result = actionresult.Result as OkObjectResult;
            var actual = result.Value as IEnumerable<Shares>;

            //Assert.NotNull(result);
            //var result = Assert.IsType<List<Shares>>(actionresult);
            
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(SampleRepo().Count(), actual.Count());
        }

        [Fact]
        public void PostTest()
        {
            var share = new Shares { Name = "ABC CO.", Price = 1000 };

            var _controller = new HomeController(_mockRepo.Object);
            var result = _controller.Post(share);
            
            Assert.IsType<OkResult>(result); 
        }

        [Fact]
        public void PostTest_BadRequest()
        {
            var share = new Shares { Name = "", Price = 1000 };

            var _controller = new HomeController(_mockRepo.Object);
            var result = _controller.Post(share);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void DisplayShare_Found()
        {
            var MockShares = SampleRepo();
            var firstShare = MockShares[0];

            _mockRepo.Setup(repo => repo.GetShares("Company One"))
                   .Returns(firstShare);

            var _controller = new HomeController(_mockRepo.Object);

            var actionresult = _controller.GetPrice("Company One");
            var result = actionresult.Result as OkObjectResult;

            //var result = Assert.IsType<Shares>(actionresult);

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(firstShare, result.Value);
        }

        [Fact]
        public void DisplayShare_NotFound()
        {
            var MockShares = SampleRepo();
            var firstShare = MockShares[0];

            _mockRepo.Setup(repo => repo.GetShares("Company One"))
                   .Returns(firstShare);

            var _controller = new HomeController(_mockRepo.Object);

            var actionresult = _controller.GetPrice("Company Zero");
            //Assert.IsType<NotFoundResult>(actionresult);


            Assert.IsType<NotFoundObjectResult>(actionresult);
        }
        
        [Fact]
        public void DisplayShare_BadRequest()
        {
            var MockShares = SampleRepo();
            var firstShare = MockShares[0];

            _mockRepo.Setup(repo => repo.GetShares("Company One"))
                   .Returns(firstShare);

            var _controller = new HomeController(_mockRepo.Object);

            var actionresult = _controller.GetPrice("");


            Assert.IsType<BadRequestObjectResult>(actionresult);
        }

        private List<Shares> SampleRepo()
        {
            List<Shares> output = new List<Shares>
            {
                new Shares
                {
                    Name = "Company One",
                    Price = 1001
                },
                new Shares
                {
                    Name = "Company two",
                    Price = 1002
                }

            };
            return output;
        }

    }
}
