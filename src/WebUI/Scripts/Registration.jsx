/*Ok, that's redundant, we will work only with Razor just for now*/

var RegistrationForm = React.createClass({
    render: function () {
        return (
            <div className="registrationForm">
                <form>
                    <AddFieldWidget />
                    <div id="fieldsAppender"></div>
                    <AddFieldWidget />
                    <ButtonsWidget />
                </form>
            </div>
        );
    }
});

/*var CivilStatusWidget = React.createClass({
   render: function () {
       return (
           <div className="civilStatusWidget">
               <div>Civil status:</div>
               <div>
                   <input type="radio" name="CivilStatus" value="Married"/>
                   <input type="radio" name="CivilStatus" value="Single"/>
                   <input type="radio" name="CivilStatus" value="Divorced"/>
               </div>
           </div>
       );
   }
});*/

/*var HasChildrenWidget = React.createClass({
   render: function () {
       return (
           <div className="hasChildrenWidget">
               <div>Children</div>
               <div>
                   <input type="radio" name="HasChildren" value="Yes"/>
                   <input type="radio" name="HasChildren" value="No"/>
               </div>
           </div>
       );
   }
});*/

var AddFieldWidget = React.createClass({
   render: function () {
       return (
           <div className="addFieldWidget">
               <h3>New family member</h3>
               <div>
                   <button id="addFieldButton">+ Add</button>
               </div>
           </div>
       );
   }
});

var InputFieldWidget = React.createClass({
   render: function () {
       return (
           <div className="inputFieldWidget" class="inputField">
               <input type="text" ref="name" placeholder="Your name"/>
               <input type="text" ref="sName" placeholder="Your second name"/>
               <select ref="gender">
                   <option value="">Gender</option>
                   <option value="M">Male</option>
                   <option value="F">Female</option>
                   <option value="N">Not defined</option>
               </select>
               <select ref="civilStat">
                   <option value="">Civil status</option>
                   <option value="Husband">Husband</option>
                   <option value="Wife">Wife</option>
                   <option value="Child">Child</option>
               </select>
               <div>
                   Responsible for the list
                   <input type="checkbox" id="responsible" ref="responsible"/>
                   <input type="text" id="email" ref="email" style="visibility: hidden"/>
               </div>
               <button style="visibility: hidden" id="delResp">Remove</button>
           </div>
       );
   }
});

var ButtonsWidget = React.createClass({
    handleSubmit: function (e) {
        e.preventDefault();
        var formData = document.getElementsByClassName('inputField');
        for (var i = 0, max = formData.length; i  < max; i ++) {
            
            
        }
    },
    render: function () {
       return (
           <div className="buttonsWidget">
               <div style="float: left">
                   <button type="reset"/>
               </div>
               <div>
                   <button type="submit" id="submitButton">Next step</button>
               </div>
           </div>
       );
   }
});

ReactDOM.Render(
    <RegistrationForm />,
    document.getElementById('content')
);

/* Some additional logic for the form */

$('#responsible').click(function() {
    if( $(this).is(':checked')) {
        $("#email").show();
    } else {
        $("#email").hide();
    }
});

$('#addFieldButton').click(function () {
    ReactDOM.render(
        <InputFieldWidget/>,
        $('#fieldsAppender')
    );
});

$('#submitButton').click(function () {
    var data = document.getElementById('#inputField');
})

