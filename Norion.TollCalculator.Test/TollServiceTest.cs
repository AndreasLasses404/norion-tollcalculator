using Moq;
using Norion.TollCalculator.Application.Service;
using Norion.TollCalculator.Domain.Service;
using Norion.TollCalculator.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norion.TollCalculator.Test;

public class TollServiceTest
{
    private readonly IMock<TollRepository> _repositoryMock;
    private readonly TollService _service;
    public TollServiceTest()
    {
        _repositoryMock = new Mock<TollRepository>();
        _service = new TollService(_repositoryMock.Object);
    }

}
