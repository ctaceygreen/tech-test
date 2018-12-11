class Orders extends React.Component {
    constructor(props) {
        super(props);
        this.state = { CustomerIds: [], Orders: [], pollInterval: 5000};
        this.LoadOrdersFromApi = this.LoadOrdersFromApi.bind(this);
    }
    render() {
        return (
            <div className="row">
                <div className="alert alert-info">
                    Note: There is no validation within the front-end yet. Of course, I would add that in a real system, but I didn't want to spend long on the front end for this coding test!
                </div>
                <div className="col col-md-12">
                    <OrdersSection pollInterval={this.state.pollInterval} Orders={this.state.Orders} />
                </div>
                <div className="col col-md-12">
                    <NewOrderSection CustomerIds={this.state.CustomerIds} addOrder={this.addOrder} />
                </div>
            </div>
        );
    }
    
    addOrder(customerId, customerName, customerCountry, customerDoB, orderAmount) {
        //Call add order, then call loadOrdersFromApi
        const xhr = new XMLHttpRequest();
        xhr.open('POST', 'api/Orders', true);
        xhr.setRequestHeader("Accept", "application/json");
        xhr.setRequestHeader("Content-type", "application/json; charset=utf-8");
        xhr.onreadystatechange = function () { // Call a function when the state changes.
            if (this.readyState === XMLHttpRequest.DONE && this.status === 200) {
                // Request finished. Do processing here.
                this.LoadOrdersFromApi();
            }
        }
        xhr.send(JSON.stringify({ customerId: customerId, customerName: customerName, customerCountry: customerCountry, dateOfBirth: customerDoB , amount: orderAmount }));
    }

    LoadOrdersFromApi() {
        const xhr = new XMLHttpRequest();
        xhr.open('get', 'api/Orders/GetOrders', true);
        xhr.onload = () => {
            const data = JSON.parse(xhr.responseText);
            this.setState({ Orders: data });
            let customerIds = [];
            for (let i = 0; i < data.length; i++) {
                customerIds.push(data[i].Id);
            }
            this.setState({ CustomerIds: customerIds });
        };
        xhr.send();
    }

    componentDidMount() {
        this.LoadOrdersFromApi();
        window.setInterval(() => this.LoadOrdersFromApi(), this.state.pollInterval);
    }
}
class OrdersSection extends React.Component {
    constructor(props) {
        super(props);
        
    }
    render() {
        return (
            <div>
                {this.props.Orders.map(item => <CustomerItem key={item.Id} item={item} />)}
            </div>
            )
    }
}

class CustomerItem extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div className="col col-md-12">
                <h3>Id: {this.props.item.Id} , Name: {this.props.item.Name} , Country: {this.props.item.Country} , DoB: {this.props.item.DateOfBirth}</h3>
                Orders:
                <table className="table table-condensed">
                    <thead>
                        <tr>
                            <th>Id</th>
                            <th>Amount</th>
                            <th>VAT</th>
                        </tr>
                    </thead>
                    <tbody>
                        {this.props.item.Orders.map(item => <CustomerOrderItem key={item.Id} item={item} />)}
                    </tbody>
                </table>
            </div>)
    }
}

class CustomerOrderItem extends React.Component {
    constructor(props) {
        super(props);
    }
    render() {
        return (
            <tr>
                <td>{this.props.item.Id}</td>
                <td>{this.props.item.Amount}</td>
                <td>{this.props.item.VAT}</td>
            </tr>
            )
    }
}

class NewOrderSection extends React.Component {
    constructor(props) {
        super(props);
        this.state = { newOrder_customerId: -1, newOrder_customerName: '', newOrder_customerCountry: '', newOrder_customerDoB: '1800/01/01' , newOrder_amount: 0 }
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
    }

    handleSubmit(event) {
        event.preventDefault();
        this.props.addOrder(this.state.newOrder_customerId, this.state.newOrder_customerName, this.state.newOrder_customerCountry, this.state.newOrder_customerDoB ,this.state.newOrder_amount);
    }

    handleInputChange(event) {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;

        this.setState({
            [name]: value
        });
    }

    render() {
        return (
            <form onSubmit={this.handleSubmit}>
                <label>
                    CustomerId:
                    <select name="newOrder_customerId" value={this.state.newOrder_customerId} onChange={this.handleInputChange}>
                        <option value='-1'>New</option>
                        {this.props.CustomerIds.map(item => <DropDownItem key={item} item={item} />)}
                    </select>
                </label>
                <label>
                    Name:
                <input
                        name="newOrder_customerName"
                        type="text"
                        value={this.state.newOrder_customerName}
                        onChange={this.handleInputChange}
                        disabled={this.state.newOrder_customerId != -1}/>
                </label>
                <label>
                    Country:
                <input
                        name="newOrder_customerCountry"
                        type="text"
                        value={this.state.newOrder_customerCountry}
                        onChange={this.handleInputChange}
                        disabled={this.state.newOrder_customerId != -1}/>
                </label>
                <label>
                    DOB:
                    <input type="date" id="start" name="newOrder_customerDoB"
                        value={this.state.newOrder_customerDoB}
                        min="1800-01-01" max="11/12/2018" onChange={this.handleInputChange}
                        disabled={this.state.newOrder_customerId != -1}/>
                </label>
                <label>
                    Amount:
                <input
                        name="newOrder_amount"
                        type="number"
                        value={this.state.newOrder_amount}
                        onChange={this.handleInputChange} />
                </label>
                <input type="submit" value="Submit" />
            </form>
            );
    }
}
class DropDownItem extends React.Component {
    render() {
        return (
            <option value={this.props.item}>{this.props.item}</option>
        );
    }
}

ReactDOM.render(
    <Orders />,
    document.getElementById('content')
);