import * as CrComLib from '@crestron/ch5-crcomlib/build_bundles/amd/cr-com-lib'

function pulse(join, timeout = 30){
    press(join)
    setTimeout(() => {
        release(join)
    }, timeout)
}
function press(join){
    console.log("Press join: ", join)
    CrComLib.publishEvent('b', `${join}`, true)
}
function release(join){
    console.log("Release join: ", join)
    CrComLib.publishEvent('b', `${join}`, false)
}
function setAnalog(join, value){
    console.log(`Set analog join ${join} to ${parseInt(value)}`)
    CrComLib.publishEvent('n', `${join}`, parseInt(value))
}
function setSerial(join, value){
    console.log(`Set serial join ${join} to ${value}`)
    CrComLib.publishEvent('s', `${join}`, value)
}

// Reusable function for allowing a press and hold
function useButton(pressed = () => {}, released = () => {}, tapped = () => {}, held = () => {}, useHold, timeout = 1000){
    let holdHandler = null

    function btnPress(){
        if(useHold){
            holdHandler = setTimeout(() => {
                holdHandler = null
                held()
            }, timeout)
        }
        else {
            pressed()
        }
    }
    function btnRelease(){
        clearTimeout(holdHandler)
        if(useHold){
            if(holdHandler){
                tapped()
            }
        }
        else {
            released()
        }
    }

    return {
        btnPress,
        btnRelease
    }
}

export {
    pulse,
    press,
    release,
    setAnalog,
    setSerial,
    useButton
}