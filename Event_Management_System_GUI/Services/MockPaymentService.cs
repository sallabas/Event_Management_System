namespace Event_Management_System_GUI.Services
{
    // Simulation Bank Response for the payment
    public class MockPaymentService
    {
        public async Task<bool> ProcessPaymentAsync(double amount, string accountName, string iban)
        {
            await Task.Delay(1500); 

            Random rnd = new Random();
            return rnd.Next(1, 100) <= 95;
        }
    }
}