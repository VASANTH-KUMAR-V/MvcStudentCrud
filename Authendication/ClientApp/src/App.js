import React, { Component } from 'react';
import { Route, Switch, Redirect } from 'react-router-dom';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import AuthModule from './components/AuthModule';
import './custom.css';

export default class App extends Component {
    state = {
        isAuthenticated: false, // Track login/signup status
    };

    handleAuthSuccess = () => {
        this.setState({ isAuthenticated: true });
    };

    render() {
        const { isAuthenticated } = this.state;

        if (!isAuthenticated) {
            // Show login/signup first
            return <AuthModule onAuthSuccess={this.handleAuthSuccess} />;
        }

        return (
            <Layout>
                <Switch>
                    <Route exact path="/" component={Home} />
                    <Route path="/counter" component={Counter} />
                    <Route path="/fetch-data" component={FetchData} />
                    <Redirect to="/" />
                </Switch>
            </Layout>
        );
    }
}
