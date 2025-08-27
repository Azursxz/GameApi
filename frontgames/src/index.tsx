import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import Card from './components/card'
import reportWebVitals from './reportWebVitals';
import FilterControls from './components/filterControl';



const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
  <React.StrictMode>
    <FilterControls/>
    <Card
          key={12}
          id={12}
          name={"Crowd City"}
          image={"https://store-images.s-microsoft.com/image/apps.28239.13922210868210363.c2ccdf5d-6051-417f-a330-8cbe27f045c1.c39a204c-6041-4bae-a9da-660a770891f0?q=90&w=177&h=177"}
          link = {"http://www.xbox.com/es-AR/games/store/crowd-city/9N0NFX4ZPFDS/0010"}
          originalPrice={5499.00}
          discount={20}
        />

  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
