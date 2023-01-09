
using UnityEngine;


/**
 * Company data and details.
 *
 * @module Companies
 * @main Companies
 * @class Company
 * @date August 14, 2013
 * @author Jaime Valle Alonso
 */
public class Company /*: ScriptableObject*/
{
	/**
		* Company name. For example: <i>IBERIA Líneas Aéreas de España, S.A.</i>
		* @attribute name
		* @type {string}
		*/
	private string companyName;
    /**
		* Company callsign. For example: <i>Iberia</i> for <i>IBERIA LAE, S.A.</i>
		* @attribute callsign
		* @type {string}
		*/
    private string callsign;
    /**
		* Company callsignCode. For example: <i>IBE</i> for <i>IBERIA LAE, S.A.</i>
		* @attribute callsignCode
		* @type {string}
		*/
    private string callsignCode;

	/**
		* @class Company
		* @constructor
		* @param {string} name Company name. For example: <i>IBERIA Líneas Aéreas de España, S.A.</i>
		* @param {string} callsign Company callsign. For example: <i>Iberia</i> for <i>IBERIA LAE, S.A.</i>
		* @param {string} callsignCode Company callsignCode. For example: <i>IBE</i> for <i>IBERIA LAE, S.A.</i>
		*/
	public Company(string companyName, string callsign, string callsignCode)
	{
		this.companyName = companyName;
		this.callsign = callsign;
		this.callsignCode = callsignCode;
	}

	public void SetCompanyName(string name)
	{
		this.companyName = name;
	}

	public string GetCompanyName() 
	{ 
		return this.companyName;
	}

    public void SetCallsign(string callsign)
    {
        this.callsign = callsign;
    }

    public string GetCallsign()
    {
        return this.callsign;
    }

    public void SetCallsignCode(string callsign)
    {
        this.callsignCode = callsignCode;
    }

    public string GetCallsignCode()
    {
        return this.callsignCode;
    }



}// end-class
