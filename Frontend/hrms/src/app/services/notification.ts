import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  // Inside NotificationService class
async confirmAction(title: string, message: string): Promise<boolean> {
  const result = await Swal.fire({
    title: title,
    text: message,
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#3085d6',
    cancelButtonColor: '#d33',
    confirmButtonText: 'Yes, Logout',
    cancelButtonText: 'Cancel'
  });
  
  return result.isConfirmed;
}

  // A small "Toast" message that appears in the top-right corner
  showToast(message: string, icon: 'success' | 'error' | 'warning' | 'info' = 'success') {
    const Toast = Swal.mixin({
      toast: true,
      position: 'top-end',
      showConfirmButton: false,
      timer: 3000,
      timerProgressBar: true,
      didOpen: (toast) => {
        toast.addEventListener('mouseenter', Swal.stopTimer)
        toast.addEventListener('mouseleave', Swal.resumeTimer)
      }
    });

    Toast.fire({
      icon: icon,
      title: message
    });
  }

  // A big, impressive Success Pop-up
  showSuccess(title: string, message: string) {
    Swal.fire({
      title: title,
      text: message,
      icon: 'success',
      confirmButtonColor: '#3085d6',
      confirmButtonText: 'Great!'
    });
  }

  // An Error Pop-up
  showError(title: string, message: string) {
    Swal.fire({
      title: title,
      text: message,
      icon: 'error',
      confirmButtonText: 'Try Again'
    });
  }
}