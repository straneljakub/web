import { useState, useEffect } from 'react';
import './App.css';

//funkce vrací tabulku log záznamů (name, surname, time)
export default function App({ url }) {

  const [data, setData] = useState();

  useEffect(() => {
    fetch(url)
      .then(r => r.json())
      .then(setData)
      .catch(console.error);
  }, [url]); // [] je dependeci array (kdy má být hook spušten), pokud je [] tak jen na začátku

  if (!data) return 'loading...';

  if (data) {
    return (
      <table className='table'>
        <thead>
          <tr>
            <td>
              Name
            </td>
            <td>
              Surname
            </td>
            <td>
              Time
            </td>
          </tr>
        </thead>
        <tbody>
          {data.map((obj, i) => {
           return <Record key={i} name={obj.name} surname={obj.surname} time={obj.time} />
          })}
        </tbody>
      </table>
    );
  }
  
  function Record({ name, surname, time }) {
    return (
      <tr>
        <td>
          {name}
        </td>
        <td>
          {surname}
        </td>
        <td>
          {time}
        </td>
      </tr>
    );
  }
}



