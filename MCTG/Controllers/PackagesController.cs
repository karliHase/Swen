using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MCTG.DataLayer.Repositories;
using MCTG.DataTransferObjects;

namespace MCTG.Controllers
{
    public class PackagesController
    {
        public static HTTPResponse CreatePackage(string body, string authToken)
        {
            if(!authToken.Equals("Basic admin-mtcgToken")) return new HTTPResponse(System.Net.HttpStatusCode.Unauthorized, "You do not have permission to create packages", false);
            else
            {
                PackageDTO? dto;
                try
                {
                    //deserialize HTTP message body and create RegisterDto object             
                    dto = JsonSerializer.Deserialize<PackageDTO>(body);
                    if (dto == null) throw new InvalidCastException("failed to parse Json");
                    PackageRepository.Instance.Create(dto);
                    
                    Console.WriteLine("Package created.");
                    return new HTTPResponse(System.Net.HttpStatusCode.OK, "Package created", false);
                }
                catch (InvalidCastException i)
                {
                    Console.WriteLine($"{i.Message}");
                    return new HTTPResponse(System.Net.HttpStatusCode.BadRequest, "Failed to create package", false);
                }
            }
        }

        public static HTTPResponse AcquirePackage(string authToken)
        {
            Guid userId = TokenRepository.Instance.GetUidbyToken(authToken);

            int gold = UserRepository.Instance.GetGoldByUID(userId);

            if (gold < 5) return new HTTPResponse(System.Net.HttpStatusCode.Forbidden, "You don't have enough gold to buy this package.", false);
            else
            {
                Guid? p_pid = PackageRepository.Instance.GetPackageIdOfFirstCard();
                if (p_pid == null) return new HTTPResponse(System.Net.HttpStatusCode.BadRequest, "No more packages available", false);
                List<CardDTO> newCards = PackageRepository.Instance.GetPackage(p_pid);
                bool result = CardRepository.Instance.AddPackage(newCards, userId.ToString());
                PackageRepository.Instance.Delete(p_pid.ToString());
                UserRepository.Instance.reduceGold(userId, gold);
            }
            return new HTTPResponse(System.Net.HttpStatusCode.OK, $"Cards added for authtoken {authToken}", false);    
        }
    }
}
