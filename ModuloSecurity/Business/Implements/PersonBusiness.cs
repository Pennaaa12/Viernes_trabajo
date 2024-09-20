using Business.Interface;
using Data.Interfaces;
using Entity.DTO;
using Entity.Model.Security;
using Microsoft.EntityFrameworkCore;

namespace Business.Implements
{
    public class PersonBusiness : IPersonBusiness
    {
        protected readonly IPersonData data;

        public PersonBusiness(IPersonData data)
        {
            this.data = data;
        }

        public async Task Delete(int id)
        {
            await this.data.Delete(id);
        }

        public async Task<IEnumerable<PersonDto>> GetAll()
        {
            IEnumerable<Person> persons = await this.data.GetAll();
            var personDtos = persons.Select(person => new PersonDto
            {
                Id = person.Id,
                First_name = person.First_name,
                Last_name = person.Last_name,
                Email = person.Email,
                Address = person.Address,
                Type_document = person.Type_document,
                Document = person.Document,
                Birth_of_date_ = person.Birth_of_date,
                Phone = person.Phone,
                State = person.State,
                CityId = person.CityId // Incluye CityId
            });
            return personDtos;
        }

        public async Task<IEnumerable<DataSelectDto>> GetAllSelect()
        {
            return await this.data.GetAllSelect();
        }

        public async Task<PersonDto> GetById(int id)
        {
            var person = await this.data.GetById(id); // Usa el método de data

            if (person == null)
            {
                return null;
            }

            return new PersonDto
            {
                Id = person.Id,
                First_name = person.First_name,
                Last_name = person.Last_name,
                Email = person.Email,
                Address = person.Address,
                Type_document = person.Type_document,
                Document = person.Document,
                Birth_of_date_ = person.Birth_of_date,
                Phone = person.Phone,
                State = person.State,
                CityId = person.CityId // Incluye CityId
            };
        }

        public Person mapearDatos(Person person, PersonDto entity)
        {
            person.Id = entity.Id;
            person.First_name = entity.First_name;
            person.Last_name = entity.Last_name;
            person.Email = entity.Email;
            person.Address = entity.Address;
            person.Type_document = entity.Type_document;
            person.Document = entity.Document;
            person.Birth_of_date = entity.Birth_of_date_;
            person.Phone = entity.Phone;
            person.State = entity.State;
            person.CityId = entity.CityId; // Asegúrate de asignar el CityId aquí también
            return person;
        }

        public async Task<Person> Save(PersonDto entity)
        {
            Person person = new Person
            {
                CreateAt = DateTime.Now,
                First_name = entity.First_name,
                Last_name = entity.Last_name,
                Email = entity.Email,
                Address = entity.Address,
                Type_document = entity.Type_document,
                Document = entity.Document,
                Birth_of_date = entity.Birth_of_date_,
                Phone = entity.Phone,
                State = entity.State,
                CityId = entity.CityId // Asegúrate de asignar el CityId
            };

            return await this.data.Save(person);
        }

        public async Task Update(PersonDto entity)
        {
            Person person = await this.data.GetById(entity.Id);
            if (person == null)
            {
                throw new Exception("Registro no encontrado");
            }
            person = this.mapearDatos(person, entity);

            await this.data.Update(person);
        }
    }
}
