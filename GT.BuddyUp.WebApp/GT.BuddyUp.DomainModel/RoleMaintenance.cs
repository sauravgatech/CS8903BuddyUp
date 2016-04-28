using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GT.BuddyUp.DomainDto;
using GT.BuddyUp.Platform.Common;
using GT.BuddyUp.Platform.Repository;
using GT.BuddyUp.EntityModel;
using Microsoft.Practices.Unity;

namespace GT.BuddyUp.DomainModel
{
    public class RoleMaintenance : IRole
    {
        private IRepository<AspNetRoles> _repRole;
        private IUnitOfWork _uow;
        
        private DomainModelResponse _roleResponse = new DomainModelResponse();

        public RoleMaintenance([Dependency("buddyup")] IUnitOfWork uow)
        {
            _uow = uow;
        }

        [InjectionMethod]
        public void Initialize(IRepository<EntityModel.Role> repRole)
        {
            _repRole = repRole.Use(_uow);
        }

        public IEnumerable<DomainDto.Role> Get(string roleCode = "")
        {
            IEnumerable<EntityModel.Role> roles = _repRole.Get(filter: u => (u.RoleCode == roleCode || roleCode == ""));
            List<DomainDto.Role> dtoRoles = new List<DomainDto.Role>();
            foreach(EntityModel.Role role in roles)
            {
                DomainDto.Role dr = new DomainDto.Role()
                {
                    roleCode = role.RoleCode,
                    roleDescription = role.RoleDescription
                };
            }
            return dtoRoles;
        }

        public DomainModelResponse Add(DomainDto.Role request)
        {
            EntityModel.Role role = new EntityModel.Role()
            {
                RoleCode = request.roleCode,
                RoleDescription = request.roleDescription,
                LastChangedTime = DateTime.UtcNow
            };
            _repRole.Add(role);
            _uow.Commit();
            _roleResponse.addResponse("Add", MessageCodes.InfoCreatedSuccessfully, "Role");
            return _roleResponse;
        }

        public DomainModelResponse Update(DomainDto.Role request)
        {
            EntityModel.Role role = _repRole.Get(filter: u => u.RoleCode == request.roleCode).FirstOrDefault();
            if (role == null)
            {
                _roleResponse.addResponse("Update", MessageCodes.ErrDoesnotExist, "role : " + request.roleCode);
                throw _roleResponse;
            }
            role.RoleDescription = request.roleDescription;
            role.LastChangedTime = DateTime.UtcNow;
            _repRole.Update(role);
            _uow.Commit();
            _roleResponse.addResponse("Update", MessageCodes.InfoSavedSuccessfully, "Role");
            return _roleResponse;
        }

        public DomainModelResponse Delete(string roleCode)
        {
            return new DomainModelResponse();
        }
    }
}
