﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZaklepTo.Infrastucture.Services.Implementations;
using AutoMapper;
using ZaklepTo.Core.Domain;
using ZaklepTo.Infrastucture.DTO;
using ZaklepTo.Infrastucture.Services.Interfaces;
using ZaklepTo.Infrastucture.DTO.OnUpdate;

namespace ZaklepTo.API.Controllers
{
    [Route("api/customers")]
    public class CustomerController : Controller
    {
        private ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllCustomers()
        {
            IEnumerable<CustomerDTO> customers = await _customerService.GetAllAsync();

            return Ok(customers);
        }

        [HttpGet("{login}")]
        public async Task<IActionResult> GetSingleCustomer(string login)
        {
            var customerDTO = await _customerService.GetAsync(login);

            if (customerDTO == null)
                return NotFound();

            return Ok(customerDTO);
        }  

        [HttpGet("{login}/toprestaurants")]
        public async Task<IActionResult> GetCustomerTopRestaurants(string login)
        {
            IEnumerable<RestaurantDTO> topRestaurantsForCustomer = 
                await _customerService.GetMostFrequentRestaurants(login);

            return Ok(topRestaurantsForCustomer);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterCustomer([FromBody] CustomerOnCreateDTO customer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _customerService.RegisterAsync(
                customer.Login,
                customer.FirstName,
                customer.LastName,
                customer.Email,
                customer.Phone,
                customer.Password
                );

            return Created($"{customer.Login}", Json(customer));
        }

        [HttpPut("{login}/update")]
        public async Task<IActionResult> UpdateCustomer([FromBody] CustomerOnUpdateDTO updatedCustomer)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var customerToUpdate = await _customerService.GetAsync(updatedCustomer.Login);

            if (customerToUpdate == null)
                return BadRequest();

            await _customerService.UpdateAsync(updatedCustomer);

            return Ok();
        }

        [HttpPut("{login}/changepassword")]
        public async Task<IActionResult> ChangeCustomerPassword([FromBody] PasswordChange passwordChange)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var customer = await _customerService.GetAsync(passwordChange.Login);

            if (customer == null)
                return BadRequest();

            await _customerService.ChangePassword(
                passwordChange.Login,
                passwordChange.OldPassword,
                passwordChange.NewPassword
                );

            return Ok();
        }
        
        [HttpDelete("{login}/remove")]
        public async Task<IActionResult> RemoveCustomer(string login)
        {
            var customer = _customerService.GetAsync(login);

            if (customer == null)
                return NotFound();

            await _customerService.DeleteAsync(login);

            return Ok();
        }
    }
}