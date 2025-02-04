import { useState, useEffect } from "react";
import AddTransaction from "./AddTransaction";
import EditableTextbox from "./EditableTextbox ";
import RotatingBudget from "./RotatingBudget";
import '../TotalExpense.css';

const Overview = () => {
    const [income, setIncome] = useState(0);
    const [totalExpenses, setTotalExpenses] = useState(0);
    const [remainingBudget, setRemainingBudget] = useState(0);
    const [rotatingBudget, setRotatingBudget] = useState([]);
    const [date, setDate] = useState(new Date().toISOString().split("T")[0])

    // Handle deleting a rotating budget item
    const handleDeleteRotatingItem = async (itemId) => {
        try {
            // Send DELETE request
            const response = await fetch(`http://localhost:5259/api/transactions/DeleteRotatingBudget/${itemId}`, {
         
                method: "DELETE",
            });
            console.log("ItemID:" ,itemId);
            if (!response.ok) {
                throw new Error(`Error deleting item: ${response.status}`);
            }

            console.log("Item deleted successfully");

            fetchData();
        } catch (error) {
            console.error("Error deleting item:", error);
        }
    };
    const fetchData = async () => {
        try {
            const incomeResponse = await fetch(`http://localhost:5259/api/transactions/Income/${date}`);
            const expensesResponse = await fetch(`http://localhost:5259/api/transactions/TotalExpenses/${date}`);
            const budgetResponse = await fetch(`http://localhost:5259/api/transactions/RamainingBudget/${date}`);
            const rotatingResponse = await fetch(`http://localhost:5259/api/transactions/RotatingBudget/${date}`);

            const incomeData = await incomeResponse.json();
            const expensesData = await expensesResponse.json();
            const budgetData = await budgetResponse.json();
            const rotatingData = await rotatingResponse.json();

            setIncome(incomeData.amount);
            setTotalExpenses(expensesData);
            setRemainingBudget(budgetData);
            setRotatingBudget(Array.isArray(rotatingData) ? rotatingData : rotatingData || []); 
        } catch (error) {
            console.error("Error fetching data:", error);
        }
    };
    useEffect(() => {
        fetchData();
    }, [date]);
    return (
        <div className="app">
            <h1 className="header">PennyWise</h1>
            <div className="dashboard">
                <div className="card overview">
                    <h2>Income</h2>
                    <EditableTextbox
                        fetchData={fetchData} date={date} />
                    <h2>Total Expenses</h2>
                    <div className="value">
                        {totalExpenses}
                    </div>
                    <h2>Remaining Budget</h2>                    
                    <div className="value">
                        {remainingBudget}
                    </div>
                </div>
                <div className="card rotating-budget">
                    <h2>Automated payments list</h2>
                    <button>Add</button>
                </div>
                <div className="card add-transaction">
                    <h2>Rotating Budget</h2>
                    <RotatingBudget rotatingBudget={rotatingBudget} onDelete={handleDeleteRotatingItem} />
                </div>
                <div className="card add-transaction">
                    <h2>Add Transaction</h2>
                    <AddTransaction onTransactionAdded={fetchData} />
                </div>

                <div className="card add-transaction">
                    <h2>History</h2>
                    <div >
                        <input className="mounth-button" type="date" value={date}
                            onChange={(e) => setDate(e.target.value) } />
                    </div>                   
                </div>
            </div>           
        </div>
    );
};

export default Overview;
